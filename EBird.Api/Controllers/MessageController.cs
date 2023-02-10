using System.Net;
using EBird.Application.Hubs;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Response;

namespace EBird.Api.Controllers;
[Route("message")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
    private readonly IGenericRepository<MessageEntity> _messageRepository;

    public MessageController(IHubContext<ChatHub> hubContext, IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<MessageEntity> messageRepository)
    {
        _hubContext = hubContext;
        _chatRoomRepository = chatRoomRepository;
        _messageRepository = messageRepository;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<MessageView>>> GetMessageById(Guid id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        var messageView = new MessageView(){
            Content = message.Content,
            Timestamp = message.Timestamp,
            FromUserId = message.SenderId.ToString(),
            FromUserName = message.Sender.Username,
            FromFullName = message.Sender.FirstName,
            RoomId = message.ChatRoomId.ToString()
        };
        var response = Response<MessageView>.Builder().SetData(messageView).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<Response<List<MessageView>>>> GetMessageByRoomId(Guid roomId)
    {
        var room = await _chatRoomRepository.FindWithCondition(x => x.Id == roomId);
        var messages = await _messageRepository.FindAllWithCondition(x => x.ChatRoomId == room.Id);
        messages = messages.OrderByDescending(x => x.Timestamp).ToList();
        var messageViews = new List<MessageView>();
        foreach(var message in messages)
        {
            var messageView = new MessageView(){
                Content = message.Content,
                Timestamp = message.Timestamp,
                FromUserId = message.SenderId.ToString(),
                FromUserName = message.Sender.Username,
                FromFullName = message.Sender.FirstName,
                RoomId = message.ChatRoomId.ToString()
            };
            messageViews.Add(messageView);
        }
        var response = Response<List<MessageView>>.Builder().SetData(messageViews).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
        
    }
    [HttpPost]
    public async Task<ActionResult<Response<MessageView>>> CreateMessage(MessageView message)
    {
        var room = await _chatRoomRepository.GetByIdAsync(Guid.Parse(message.RoomId));
        await _messageRepository.CreateAsync(new MessageEntity(){
            Content = message.Content,
            Timestamp = message.Timestamp,
            SenderId = Guid.Parse(message.FromUserId),
            ChatRoomId = room.Id
        });
        await _hubContext.Clients.Group(message.RoomId).SendAsync("newMessage", message);
        var response = Response<MessageView>.Builder().SetData(message).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
    [HttpDelete("{messageId}")]
    public async Task<ActionResult<Response<MessageEntity>>> DeleteMessage(Guid messageId)
    {
        var mess = await _messageRepository.FindWithCondition(x => x.Id == messageId);
        await _messageRepository.DeleteSoftAsync(messageId);
        await _hubContext.Clients.Group(mess.ChatRoomId.ToString()).SendAsync("ReceiveMessage", messageId.ToString());
        var response = Response<MessageEntity>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
}