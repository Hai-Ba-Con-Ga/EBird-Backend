using System.Security.Claims;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace EBird.Application.Hubs;
public class ChatHub : Hub
{
    private readonly static string _Connections = "Connections";
    private readonly static Dictionary<UserConnection, List<string>> OnlineUsers = new Dictionary<UserConnection, List<string>>();
    // public ChatHub(Dictionary<string, UserConnection> ConnectionsMap)
    // {
    //     _Connections = "Connections";
    //     _ConnectionsMap = ConnectionsMap;
    // }
    // public override Task OnDisconnectedAsync(Exception exception)
    // {
    //     if (_ConnectionsMap.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
    //     {
    //         _ConnectionsMap.Remove(Context.ConnectionId);
    //         Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", $"{userConnection.UserId} has left");
    //         SendUsersConnected(userConnection.RoomId);
    //     }

    //     return base.OnDisconnectedAsync(exception);
    // }

    // public Task SendUsersConnected(string roomId)
    // {
    //     var users = _ConnectionsMap.Values
    //     .Where(c => c.RoomId == roomId)
    //         .Select(c => c.UserId);

    //     return Clients.Group(roomId).SendAsync("UsersInRoom", users);
    // }
    // public async Task JoinRoom(UserConnection userConnection)
    // {
    //     await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomId);

    //     _ConnectionsMap[Context.ConnectionId] = userConnection;

    //     await Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", $"{userConnection.UserId} has joined {userConnection.RoomId}");

    //     await SendUsersConnected(userConnection.RoomId);
    // }
    // public async Task SendMessage(string message)
    // {
    //     if (_ConnectionsMap.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
    //     {
    //         await Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", userConnection.UserId, message);
    //     }
    // }
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

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var chatRoomId = httpContext.Request.Query["chatRoomId"].ToString();
        var rawUserId = httpContext.Request.Query["userId"].ToString();
        // var rawUserId = this.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (string.IsNullOrEmpty(rawUserId))
        // {
        //     throw new ArgumentException("User Id is not valid");
        // }
        Guid userId = Guid.Parse(rawUserId);
        Guid chatRoomIdParse = Guid.Parse(chatRoomId);
        if (string.IsNullOrEmpty(chatRoomId))
        {
            throw new ArgumentException("Chat Room Id is not valid");
        }
        var checkJoinChatRoom = await _chatRoomRepository.FindWithCondition(x => x.Id == chatRoomIdParse && x.Participants.Any(y => y.AccountId == userId));
        if (checkJoinChatRoom == null)
        {
            var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomIdParse);
            var participant = new ParticipantEntity()
            {
                AccountId = userId,
                ChatRoomId = chatRoomIdParse
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
        await Clients.Group(chatRoomId).SendAsync("UserActive", userView, $"{userView.FullName} has joined {chatRoomId}");
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var httpContext = Context.GetHttpContext();
            var chatRoomId = httpContext.Request.Query["chatRoomId"].ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            var rawUserId = httpContext.Request.Query["userId"].ToString();

            // var rawUserId = this.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // if (string.IsNullOrEmpty(rawUserId))
            // {
            //     throw new ArgumentException("User Id is not valid");
            // }
            Guid userId = Guid.Parse(rawUserId);


            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            var userView = new UserView()
            {
                UserId = userId.ToString(),
                FullName = account.FirstName + " " + account.LastName,
                UserName = account.Username ?? "",
                CurrentRoomId = chatRoomId

            };
            await Clients.Group(chatRoomId).SendAsync("UserActive", userView, $"{userView.FullName} has left {chatRoomId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("send message =============================== \n %s", ex.Message);

            Console.WriteLine(ex.Message);
        }
    }
    public async Task SendMessage(string message)
    {
        // var chatRoomId = OnlineUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key.ChatRoomId;
        // var rawUserId = this.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (string.IsNullOrEmpty(rawUserId))
        // {
        //     throw new ArgumentException("User Id is not valid");
        // }
        try
        {
            var httpContext = Context.GetHttpContext();

            var chatRoomId = httpContext.Request.Query["chatRoomId"].ToString();

            var rawUserId = Context.GetHttpContext().Request.Query["userId"].ToString();
            Console.WriteLine(rawUserId);
            Guid userId = Guid.Parse(rawUserId);
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
                await Clients.Group(chatRoomId).SendAsync("NewMessage", userId, newMessage);
                await _messageRepository.CreateAsync(newMessage);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("send message =============================== \n %s", e.Message);
        }
    }

}