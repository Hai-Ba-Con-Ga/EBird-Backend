using System.Net;
using System.Security.Claims;
using EBird.Application.Extensions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace EBird.Application.Hubs;
[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub : Hub
{
    private readonly static string _Connections = "Connections";
    private readonly static Dictionary<UserConnection, List<string>> OnlineUsers = new Dictionary<UserConnection, List<string>>();

    private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
    private readonly IGenericRepository<AccountEntity> _accountRepository;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly IGenericRepository<ParticipantEntity> _participantRepository;



    public ChatHub(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<MessageEntity> messageRepository, IGenericRepository<ParticipantEntity> participantRepository)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
    }
    public override async Task OnConnectedAsync()
    {
        try
        {
            var chatRoomId = Context.GetHttpContext().Request.Query["chatRoomId"].ToString();

            var userId = Context.User.GetUserId();

            var checkJoinChatRoom = await _chatRoomRepository.FindWithCondition(x => x.Id.ToString() == chatRoomId && x.Participants.Any(y => y.AccountId == userId));
            if (checkJoinChatRoom == null)
            {

                var participant = new ParticipantEntity()
                {
                    AccountId = userId,
                    ChatRoomId = Guid.Parse(chatRoomId)
                };

                await _participantRepository.CreateAsync(participant);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            var userView = new UserView()
            {
                UserId = userId.ToString(),
                FullName = account.FirstName + " " + account.LastName,
                UserName = account.Username ?? "",
                CurrentRoomId = chatRoomId

            };
            await Clients.Group(chatRoomId).SendAsync(HubEvents.UserActive, userView, $"{userView.FullName} has joined {chatRoomId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var chatRoomId = Context.GetHttpContext().Request.Query["chatRoomId"].ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            var userId = Context.User.GetUserId();
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            var userView = new UserView()
            {
                UserId = userId.ToString(),
                FullName = account.FirstName + " " + account.LastName,
                UserName = account.Username ?? "",
                CurrentRoomId = chatRoomId

            };
            await Clients.Group(chatRoomId).SendAsync(HubEvents.UserActive, userView, $"{userView.FullName} has left {chatRoomId}");
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }
    }
    public async Task SendMessage(string message)
    {
        try
        {
            var chatRoomId = Context.GetHttpContext().Request.Query["chatRoomId"].ToString();
            var userId = Context.User.GetUserId();
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            if (chatRoomId != null)
            {
                var newMessage = new MessageEntity()
                {
                    Content = message,
                    ChatRoomId = Guid.Parse(chatRoomId),
                    SenderId = userId,
                    Timestamp = DateTime.Now
                };
                await Clients.Group(chatRoomId).SendAsync(HubEvents.NewMessage, userId, newMessage);
                await _messageRepository.CreateAsync(newMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    public Task<bool> UserConnected(UserConnection user, string connectionId)
    {
        bool isOnline = false;
        lock (OnlineUsers)
        {
            var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.ChatRoomId == user.ChatRoomId);
            if (temp.Key == null)
            {
                OnlineUsers.Add(user, new List<string>() { connectionId });
                isOnline = true;
            }
            else if (OnlineUsers.ContainsKey(temp.Key))
            {
                OnlineUsers[temp.Key].Add(connectionId);
            }
        }
        return Task.FromResult(isOnline);
    }
    public Task<bool> UserDisconnected(UserConnection user, string connectionId)
    {
        bool isOffline = false;
        lock (OnlineUsers)
        {
            var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.ChatRoomId == user.ChatRoomId);
            if (temp.Key == null)
                return Task.FromResult(isOffline);

            OnlineUsers[temp.Key].Remove(connectionId);
            if (OnlineUsers[temp.Key].Count == 0)
            {
                OnlineUsers.Remove(temp.Key);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }
    public Task<UserConnection[]> GetOnlineUsers(string chatRoomId)
    {
        UserConnection[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.Where(u => u.Key.ChatRoomId == chatRoomId).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }
    public Task<List<string>> GetConnectionsForUser(UserConnection user)
    {
        List<string> connectionIds = new List<string>();
        lock (OnlineUsers)
        {
            var temp = OnlineUsers.SingleOrDefault(x => x.Key.UserId == user.UserId && x.Key.ChatRoomId == user.ChatRoomId);
            if (temp.Key != null)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(temp.Key);
            }
        }

        return Task.FromResult(connectionIds);
    }
}