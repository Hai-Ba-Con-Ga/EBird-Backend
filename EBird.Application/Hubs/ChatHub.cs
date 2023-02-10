using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace EBird.Application.Hubs;
public class ChatHub : Hub
{
    private static readonly List<UserView> _Connections = new List<UserView>();
    private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
    private readonly IGenericRepository<ParticipantEntity> _participantRepository;
    private readonly IGenericRepository<AccountEntity> _accountRepository;

    public ChatHub(IGenericRepository<MessageEntity> messageRepository, IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<ParticipantEntity> participantRepository)
    {
        _messageRepository = messageRepository;
        _chatRoomRepository = chatRoomRepository;
        _participantRepository = participantRepository;
    }
    private string IdentityName
    {
        get { return Context.User.Identity.Name; }
    }

    public async Task SendPrivate(string receiverId, string message)
    {
        if (_ConnectionsMap.TryGetValue(receiverId, out string userId))
        {
            var sender = _Connections.Where(u => u.UserId == IdentityName).FirstOrDefault();
            if (!string.IsNullOrEmpty(message.Trim()))
            {
                var messageView = new MessageView()
                {
                    FromUserId = sender.UserId,
                    Content = message,
                    FromFullName = sender.FullName,
                    FromUserName = sender.UserName,
                    Timestamp = DateTime.Now
                };
                await Clients.Client(userId).SendAsync("newMessage", messageView);
                await Clients.Caller.SendAsync("newMessage", messageView);
            }
        }
    }
    public async Task Join(string roomId)
    {
        try
        {
            var user = _Connections.Where(u => u.UserId == IdentityName).FirstOrDefault();
            if (user != null && user.CurrentRoomId != roomId)
            {
                // Remove user from others list
                if (!string.IsNullOrEmpty(user.CurrentRoomId))
                    await Clients.OthersInGroup(user.CurrentRoomId).SendAsync("removeUser", user);

                // Join to new chat room
                await Leave(user.CurrentRoomId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                user.CurrentRoomId = roomId;

                // Tell others to update their list of users
                await Clients.OthersInGroup(roomId).SendAsync("addUser", user);
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
        }
    }
    public async Task Leave(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }
    public IEnumerable<UserView> GetUsers(string roomId)
    {
        return _Connections.Where(u => u.CurrentRoomId == roomId).ToList();
    }
    public override async Task OnConnectedAsync()
    {
        try
        {
            var account = await _accountRepository.FindWithCondition(c => c.Id.ToString() == IdentityName);
            var user = new UserView()
            {
                UserId = account.Id.ToString(),
                UserName = account.Username,
                FullName = account.FirstName

            };
            if (!_Connections.Any(u => u.UserId == IdentityName))
            {
                _Connections.Add(user);
                _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
            }
            await Clients.Caller.SendAsync("getProfile", user);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
        }
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var user = _Connections.Where(u => u.UserId == IdentityName).FirstOrDefault();
        if (user != null)
        {
            _Connections.Remove(user);
            _ConnectionsMap.Remove(IdentityName);
            await Clients.OthersInGroup(user.CurrentRoomId).SendAsync("removeUser", user);
        }
        await base.OnDisconnectedAsync(exception);
    }


}