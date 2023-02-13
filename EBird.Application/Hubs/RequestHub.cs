using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Extensions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Application.Model.Request;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EBird.Application.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RequestHub : Hub
    {
        private readonly static string _Connections = "Connections";
        private readonly static Dictionary<RequestUserConnection, List<string>> OnlineUsers = new Dictionary<RequestUserConnection, List<string>>();
        private readonly IGenericRepository<RequestEntity> _requestRepository;
        private readonly IGenericRepository<GroupEntity> _groupRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;

        private IWapperRepository _repository;

        private IMapper _mapper;
        public RequestHub(IGenericRepository<RequestEntity> requestRepository, IGenericRepository<GroupEntity> groupRepository, IWapperRepository repository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _groupRepository = groupRepository;
            _repository = repository;
            _mapper = mapper;
        }
        public override async Task OnConnectedAsync()
    {
        try
        {
            var groupIdRaw = Context.GetHttpContext().Request.Query["groupId"].ToString().ToLower();

            var userId = Context.User.GetUserId();
            var groupId = Guid.Parse(groupIdRaw);

            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupIdRaw);
            await UserConnected(new RequestUserConnection() { UserId = userId,  GroupId= groupId}, Context.ConnectionId);
            var account = await _accountRepository.GetByIdActiveAsync(userId);

            await Clients.Group(groupIdRaw).SendAsync(HubEvents.UserActive, $"{account.FirstName} {account.LastName} has joined {groupId}");
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
                var groupId = OnlineUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key.GroupId;
                var userId = Context.User.GetUserId();
                var isOffline = await UserDisconnected(new RequestUserConnection() { UserId = userId, GroupId = groupId }, Context.ConnectionId);
                if (isOffline)
                {
                    var account = await _accountRepository.GetByIdActiveAsync(userId);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString().ToLower());
                    await Clients.Group(groupId.ToString().ToLower()).SendAsync(HubEvents.UserActive, $"{account.FirstName} {account.LastName} has left {groupId}");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        public async Task SendRequest(RequestCreateDTO requestDto)
        {
            var groupId = Guid.Parse(Context.GetHttpContext().Request.Query["groupId"].ToString());
            requestDto.GroupId = groupId;
            requestDto.CreatedById = Context.User.GetUserId();
            if (requestDto == null)
            {
                throw new BadRequestException("Request cannot be null");
            }
            await RequestValidation.ValidateCreateRequest(requestDto, _repository);


            var requestEntity = _mapper.Map<RequestEntity>(requestDto);
            await Clients.Group(requestDto.GroupId.ToString()).SendAsync(HubEvents.ReceiveRequest, requestDto);
            await _repository.Request.CreateRequest(requestEntity);
        }
        public Task<bool> UserConnected(RequestUserConnection user, string connectionId)
        {
            bool isOnline = false;
            lock (OnlineUsers)
            {
                var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.GroupId == user.GroupId);
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
        public Task<bool> UserDisconnected(RequestUserConnection user, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {

                var temp = OnlineUsers.FirstOrDefault(x => x.Key.UserId == user.UserId && x.Key.GroupId == user.GroupId);

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
        public Task<RequestUserConnection[]> GetOnlineUsers(Guid groupId)
        {
            RequestUserConnection[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.Where(u => u.Key.GroupId == groupId).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }
        public Task<List<string>> GetConnectionsForUser(RequestUserConnection user)
        {
            List<string> connectionIds = new List<string>();
            lock (OnlineUsers)
            {
                var temp = OnlineUsers.SingleOrDefault(x => x.Key.UserId == user.UserId && x.Key.GroupId == user.GroupId);
                if (temp.Key != null)
                {
                    connectionIds = OnlineUsers.GetValueOrDefault(temp.Key);
                }
            }

            return Task.FromResult(connectionIds);
        }
    }

}