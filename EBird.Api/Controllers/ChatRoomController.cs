using System.Net;
using System.Security.Claims;
using EBird.Application.Hubs;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Response;

namespace EBird.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("chat-room")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;

        public ChatRoomController(IGenericRepository<ChatRoomEntity> chatRoomRepository, IHubContext<ChatHub> hubContext)
        {
            _chatRoomRepository = chatRoomRepository;
            _hubContext = hubContext;
        }

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
                var chatRooms = await _chatRoomRepository.FindAllWithCondition(x => x.Participants.Any(y => y.AccountId == accId));
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
            var chatRoom = await _chatRoomRepository.GetByIdAsync(id);
            var response = new Response<ChatRoomEntity>();
            response = Response<ChatRoomEntity>.Builder().SetData(chatRoom).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPost]
        public async Task<ActionResult<Response<string>>> CreateChatRoomGroup(CreateChatRoomGroup room)
        {
            var newChatRoom = new ChatRoomEntity()
            {
                Name = room.Name,
                TypeChatRoom = room.TypeChatRoom
            };

            await _chatRoomRepository.CreateAsync(newChatRoom);
            await _hubContext.Clients.All.SendAsync("addChatRoom", new RoomView()
            {
                Id = newChatRoom.Id.ToString(),
                Name = room.Name
            });
            var response = Response<string>.Builder().SetMessage("Created Successfully").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPost("add-participant")]
        public async Task<ActionResult<Response<string>>> AddParticipant(AddParticipant participant)
        {
            var chatRoom = await _chatRoomRepository.GetByIdAsync(participant.ChatRoomId);
            var response = new Response<string>();
            if (chatRoom == null)
            {
                response = Response<string>.Builder().SetMessage("Chat room not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);
                return StatusCode((int)HttpStatusCode.NotFound, response);
            }
            var participantEntity = new ParticipantEntity();
            participantEntity.ChatRoomId = participant.ChatRoomId;
            foreach (var item in chatRoom.Participants)
            {
                participantEntity.AccountId = item.AccountId;
                chatRoom.Participants.Add(participantEntity);
            }
            await _chatRoomRepository.UpdateAsync(chatRoom);
            await _hubContext.Clients.Group(participant.ChatRoomId.ToString()).SendAsync("addParticipant", participant);
            response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteChatRoom(Guid id)
        {
            var deleteChatRoom = await _chatRoomRepository.DeleteSoftAsync(id);
            await _hubContext.Clients.All.SendAsync("removeChatRoom", id.ToString());
            await _hubContext.Clients.Group(id.ToString()).SendAsync("onRoomDeleted");
            var response = Response<string>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


    }

}