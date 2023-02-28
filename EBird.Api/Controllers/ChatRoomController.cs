using System.Net;
using System.Security.Claims;
using AutoMapper;
using EBird.Application.Hubs;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Response;
using Newtonsoft.Json;

namespace EBird.Api.Controllers
{

    [Route("chat-room")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {

        private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IGenericRepository<ParticipantEntity> _participantRepository;
        private readonly IGenericRepository<RoomEntity> _roomRepository;
        private readonly IGenericRepository<RequestEntity> _requestRepository;
        private readonly IGenericRepository<GroupEntity> _groupRepository;
        private readonly IMapper _mapper;


        public ChatRoomController(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<ParticipantEntity> participantRepository, IGenericRepository<RoomEntity> roomRepository, IGenericRepository<RequestEntity> requestRepository, IGenericRepository<GroupEntity> groupRepository, IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository;
            _accountRepository = accountRepository;
            _participantRepository = participantRepository;
            _roomRepository = roomRepository;
            _requestRepository = requestRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<Response<List<ChatRoomEntity>>>> GetChatRoom()
        {

            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new Response<List<ChatRoomEntity>>();
            if (rawId == null)
            {
                response = Response<List<ChatRoomEntity>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Account not found");
                return StatusCode((int)response.StatusCode, response);
            }
            try
            {
                Guid accId = Guid.Parse(rawId);
                // var chatRooms = (await _chatRoomRepository.WhereAsync(x => x.Participants.Any(y => y.AccountId == accId), "Participants.Account")).ToList();
                var chatRooms = await _chatRoomRepository.FindAllWithCondition(x => x.Participants.Any(y => y.AccountId == accId));
                foreach (var item in chatRooms)
                {
                    item.Participants = await _participantRepository.WhereAsync(x => x.ChatRoomId == item.Id);
                }
                
                response = Response<List<ChatRoomEntity>>.Builder().SetData(chatRooms).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = Response<List<ChatRoomEntity>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ChatRoomEntity>>> GetChatRoomById(Guid id)
        {
          
            var response = new Response<ChatRoomEntity>();
            try
            {
                var chatRoom = await _chatRoomRepository.GetByIdActiveAsync(id);
                chatRoom.Participants = await _participantRepository.WhereAsync(x => x.ChatRoomId == id);
                response = Response<ChatRoomEntity>.Builder().SetData(chatRoom).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ChatRoomEntity>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("get-by-ref")]
        public async Task<ActionResult<Response<ChatRoomEntity>>> GetChatRoomByRef(Guid refId)
        {
            // var chatRoom = (await _chatRoomRepository.WhereAsync(x => x.ReferenceId == refId, "Participants.Account")).FirstOrDefault();
            var response = new Response<ChatRoomEntity>();
            try
            {
                var chatRoom = await _chatRoomRepository.FindWithCondition(x => x.ReferenceId == refId);
                chatRoom.Participants = await _participantRepository.WhereAsync(x => x.ChatRoomId == chatRoom.Id);
                response = Response<ChatRoomEntity>.Builder().SetData(chatRoom).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ChatRoomEntity>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPost()]
        public async Task<ActionResult<Response<string>>> CreateChatRoom(CreateChatRoomGroup room)
        {
            var checkRef = new object();
            var response = new Response<string>();
            if (room.TypeChatRoom == TypeChatRoom.Request)
            {
                checkRef = await _requestRepository.GetByIdActiveAsync(room.ReferenceId);
            }
            else if (room.TypeChatRoom == TypeChatRoom.Room)
            {
                checkRef = await _roomRepository.GetByIdActiveAsync(room.ReferenceId);
            }
            else if (room.TypeChatRoom == TypeChatRoom.Group)
            {
                checkRef = await _groupRepository.GetByIdActiveAsync(room.ReferenceId);
            }
            if (checkRef == null)
            {
                response = Response<string>.Builder().SetMessage("Reference not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                return StatusCode((int)HttpStatusCode.NotFound, response);
            }

            var newChatRoom = new ChatRoomEntity()
            {
                Name = room.Name,
                ReferenceId = room.ReferenceId,
                TypeChatRoom = room.TypeChatRoom
            };

            await _chatRoomRepository.CreateAsync(newChatRoom);

            response = Response<string>.Builder().SetMessage("Created Successfully").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


        // [HttpPost("create-private")]
        // public async Task<ActionResult<Response<string>>> CreateChatRoomPrivate(CreateChatRoomPrivate room)
        // {
        //     var account = await _accountRepository.GetByIdActiveAsync(room.ReceiverId);
        //     var response = new Response<string>();
        //     if (account == null)
        //     {
        //         response = Response<string>.Builder().SetMessage("Account not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
        //         return StatusCode((int)HttpStatusCode.NotFound, response);
        //     }
        //     var newChatRoom = new ChatRoomEntity()
        //     {
        //         Name = account.FirstName + " " + account.LastName,
        //         TypeChatRoom = room.TypeChatRoom,
        //         Participants = new List<ParticipantEntity>()
        //         {
        //             new ParticipantEntity()
        //             {
        //                 AccountId = room.ReceiverId,
        //                 ChatRoomId = room.ReceiverId
        //             },
        //             new ParticipantEntity()
        //             {
        //                 AccountId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
        //                 ChatRoomId =  room.ReceiverId
        //             }
        //         }
        //     };

        //     await _chatRoomRepository.CreateAsync(newChatRoom);

        //     response = Response<string>.Builder().SetMessage("Created Successfully").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        //     return StatusCode((int)HttpStatusCode.OK, response);
        // }
        [HttpPost("add-participant")]
        public async Task<ActionResult<Response<string>>> AddParticipant(AddParticipant participant)
        {
            var chatRoom = await _chatRoomRepository.FindWithCondition(x => x.ReferenceId == participant.ReferenceId);
            var response = new Response<string>();
            if (chatRoom == null)
            {
                response = Response<string>.Builder().SetMessage("Chat room not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                return StatusCode((int)HttpStatusCode.NotFound, response);
            }
            foreach (var chatRoomMember in participant.AccountId)
            {
                var acc = await _accountRepository.GetByIdAsync(chatRoomMember);
                if (acc == null)
                {
                    response = Response<string>.Builder().SetMessage("Account not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                    return StatusCode((int)HttpStatusCode.NotFound, response);
                }
                var checkExist = await _participantRepository.FindWithCondition(x => x.AccountId == chatRoomMember && x.ChatRoomId == chatRoom.Id);
                if (checkExist != null)
                {
                    response = Response<string>.Builder().SetMessage("Account already in chat room").SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest);
                    return StatusCode((int)HttpStatusCode.BadRequest, response);
                }
                var newParticipant = new ParticipantEntity()
                {
                    AccountId = chatRoomMember,
                    ChatRoomId = chatRoom.Id
                };
                await _participantRepository.CreateAsync(newParticipant);
            }
            response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteChatRoom(Guid id)
        {
            var deleteChatRoom = await _chatRoomRepository.DeleteSoftAsync(id);
            var response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


    }

}