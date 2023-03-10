using System.Net;
using System.Security.Claims;
using AutoMapper;
using EBird.Application.Extensions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using EBird.Application.Services;
using EBird.Application.Services.IServices;

namespace EBird.Application.Hubs;
// [Authorize]
//    [Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub : Hub
{
    private readonly static string _Connections = "Connections";
    private readonly static Dictionary<UserConnection, List<string>> OnlineUsers = new Dictionary<UserConnection, List<string>>();

    private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
    private readonly IGenericRepository<AccountEntity> _accountRepository;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly IGenericRepository<ParticipantEntity> _participantRepository;
    private readonly IMapper _mapper;
    private readonly IAuthenticationServices _authenticationServices;



    public ChatHub(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<MessageEntity> messageRepository, IGenericRepository<ParticipantEntity> participantRepository, IMapper mapper, IAuthenticationServices authenticationServices)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
        _authenticationServices = authenticationServices;
        _mapper = mapper;
    }
    public async Task First()
    {
        try
        {
            // var user = _authenticationServices.GetAccountById(new Guid("c395e8ed-33dc-4659-2c61-08db07458949"));
            var claims = new List<Claim>
            {
        //    await  Clients.All.SendAsync("First","OK");
                new Claim(ClaimTypes.Sid, "c395e8ed-33dc-4659-2c61-08db07458949")
            };
            var userIdentity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(userIdentity);

            Context.User.AddIdentity(userIdentity);
            await Clients.All.SendAsync("First", claims);
            //    await  Clients.All.SendAsync("First","OK");
        }
        catch (Exception ex)
        {
            await Clients.All.SendAsync("First", ex);

        }
    }
    public async Task Second()
    {

        await Clients.All.SendAsync("First", Context.User.Claims);
    }
    public override async Task OnConnectedAsync()
    {
        try
        {
            var referenceIdRaw = Context.GetHttpContext().Request.Query["referenceId"].ToString().ToLower();
            var userIdRaw = Context.GetHttpContext().Request.Query["userId"].ToString().ToLower();
            var type = Context.GetHttpContext().Request.Query["type"].ToString().ToLower();
            Console.WriteLine("===================");

            Console.WriteLine("userIdRaw: " + userIdRaw);
            Console.WriteLine("referenceIdRaw: " + referenceIdRaw);

            // var userId = Context.User.GetUserId();
            Context.User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userIdRaw) }));
            var userId = Guid.Parse(userIdRaw);
            var referenceId = Guid.Parse(referenceIdRaw);
            Console.WriteLine("===================");

            var chatRoom = await _chatRoomRepository.FindWithCondition(x => x.ReferenceId == referenceId);
            var checkJoinChatRoom = await _chatRoomRepository.FindWithCondition(x => x.ReferenceId == referenceId && x.Participants.Any(y => y.AccountId == userId));
            if (checkJoinChatRoom == null)
            {

                var participant = new ParticipantEntity()
                {
                    AccountId = userId,
                    ChatRoomId = chatRoom.Id,
                };

                await _participantRepository.CreateAsync(participant);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, referenceIdRaw);
            await UserConnected(new UserConnection() { UserId = userId, ReferenceId = referenceId }, Context.ConnectionId);
            var account = await _accountRepository.GetByIdActiveAsync(userId);

            await Clients.Group(referenceIdRaw).SendAsync(HubEvents.UserActive, $"{account.FirstName} {account.LastName} has joined {referenceId}");
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
            var referenceId = OnlineUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key.ReferenceId;
            var userId = Context.User.GetUserId();
            var isOffline = await UserDisconnected(new UserConnection() { UserId = userId, ReferenceId = referenceId }, Context.ConnectionId);
            if (isOffline)
            {
                var account = await _accountRepository.GetByIdActiveAsync(userId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, referenceId.ToString().ToLower());
                await Clients.Group(referenceId.ToString().ToLower()).SendAsync(HubEvents.UserActive, $"{account.FirstName} {account.LastName} has left {referenceId}");
            }
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
            var referenceId = OnlineUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key.ReferenceId;
            var chatRoomId =  _chatRoomRepository.FindWithCondition(x => x.ReferenceId == referenceId).Result.Id;
            var userId = Context.User.GetUserId();
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            if ( referenceId != null)
            {   
                var newMessage = new MessageEntity()
                {
                    Content = message,
                    ChatRoomId = chatRoomId,
                    SenderId = userId,
                    Timestamp = DateTime.Now
                };
                await Clients.Group( referenceId.ToString().ToLower()).SendAsync(HubEvents.NewMessage, account, newMessage);
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
            var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.ReferenceId == user.ReferenceId);
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

            var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.ReferenceId == user.ReferenceId);

            if (temp.Key == null)
            {
                Console.WriteLine("User not found");
                return Task.FromResult(isOffline);
            }

            OnlineUsers[temp.Key].Remove(connectionId);
            if (OnlineUsers[temp.Key].Count == 0)
            {
                OnlineUsers.Remove(temp.Key);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }
    public Task<UserConnection[]> GetOnlineUsers(Guid referenceId)
    {
        UserConnection[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.Where(u => u.Key.ReferenceId == referenceId).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }
    public Task<List<string>> GetConnectionsForUser(UserConnection user)
    {
        List<string> connectionIds = new List<string>();
        lock (OnlineUsers)
        {
            var temp = OnlineUsers.SingleOrDefault(x => x.Key.UserId == user.UserId && x.Key.ReferenceId == user.ReferenceId);
            if (temp.Key != null)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(temp.Key);
            }
        }

        return Task.FromResult(connectionIds);
    }
}