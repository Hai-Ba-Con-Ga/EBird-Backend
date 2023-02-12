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
    private readonly IGenericRepository<AccountEntity> _accountRepository;


    public MessageController(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<MessageEntity> messageRepository, IHubContext<ChatHub> hubContext)
    {
        _chatRoomRepository = chatRoomRepository;
        _hubContext = hubContext;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<MessageView>>> GetMessageById(Guid id)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        if (message == null)
        {
            return Response<MessageView>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        var messageView = new MessageView()
        {
            Content = message.Content,
            Timestamp = message.Timestamp,
            FromUserId = message.SenderId,
            FromUserName = message.Sender.Username,
            FromFullName = message.Sender.FirstName,
            RoomId = message.ChatRoomId
        };
        var response = Response<MessageView>.Builder().SetData(messageView).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<Response<List<MessageView>>>> GetMessageByRoomId(Guid roomId)
    {

        var messages = await _messageRepository.FindAllWithCondition(x => x.ChatRoomId == roomId);
        if (messages == null)
        {
            return Response<List<MessageView>>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        messages = messages.OrderByDescending(x => x.Timestamp).ToList();
        var messageViews = new List<MessageView>();
        foreach (var message in messages)
        {
            var messageView = new MessageView()
            {
                Content = message.Content,
                Timestamp = message.Timestamp,
                FromUserId = message.SenderId,
                FromUserName = message.Sender.Username,
                FromFullName = message.Sender.FirstName,
                RoomId = message.ChatRoomId
            };
            messageViews.Add(messageView);
        }
        var response = Response<List<MessageView>>.Builder().SetData(messageViews).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);

    }
    [HttpPost]
    public async Task<ActionResult<Response<MessageView>>> CreateMessage([FromBody] CreateMessage message)
    {
        var response = new Response<MessageView>();
        try
        {

            var room = await _chatRoomRepository.GetByIdAsync(message.RoomId);
            var account = await _accountRepository.GetByIdActiveAsync(message.FromUserId);
            if (room == null || account == null)
            {
                response = Response<MessageView>.Builder().SetMessage("Success").SetSuccess(false).SetStatusCode((int)HttpStatusCode.OK);
                return StatusCode((int)HttpStatusCode.OK, response);
            }
            var newMessage = new MessageEntity()
            {
                Content = message.Content,
                Timestamp = message.Timestamp,
                SenderId = account.Id,
                ChatRoomId = room.Id,
            };
            await _messageRepository.CreateAsync(newMessage);
            response = Response<MessageView>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            response = Response<MessageView>.Builder().SetSuccess(false).SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
        }

        return StatusCode((int)HttpStatusCode.OK, response);
    }

    [HttpDelete("{messageId}")]
    public async Task<ActionResult<Response<MessageEntity>>> DeleteMessage(Guid messageId)
    {
        var mess = await _messageRepository.FindWithCondition(x => x.Id == messageId);
        await _messageRepository.DeleteSoftAsync(messageId);
        var response = Response<MessageEntity>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
}