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
                var groupId = Context.GetHttpContext().Request.Query["groupId"].ToString();

                var userId = Context.User.GetUserId();


                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

                await Clients.Group(groupId).SendAsync(HubEvents.UserActive, $"{userId} has joined {groupId}");
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
                var groupId = Context.GetHttpContext().Request.Query["groupId"].ToString();

                var userId = Context.User.GetUserId();

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);

                await Clients.Group(groupId).SendAsync(HubEvents.UserActive, $"{userId} has left {groupId}");
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
    }
}