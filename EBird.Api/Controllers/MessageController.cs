using System.Net;
using AutoMapper;
using EBird.Application.Hubs;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
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
    private readonly IMapper _mapper;


    public MessageController(IHubContext<ChatHub> hubContext, IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<MessageEntity> messageRepository, IGenericRepository<AccountEntity> accountRepository, IMapper mapper)
    {
        _hubContext = hubContext;
        _chatRoomRepository = chatRoomRepository;
        _messageRepository = messageRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<MessageView>>> GetMessageById(Guid id)
    {
        var message = await _messageRepository.GetByIdActiveAsync(id);
        message.Sender = await _accountRepository.FindWithCondition(a => a.Id == message.SenderId);
        var response = new Response<MessageView>();
        if (message == null)
        {
            response = Response<MessageView>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        Console.WriteLine(message.Sender.FirstName);
        var messageView = new MessageView()
        {
            Content = message.Content,
            Timestamp = message.Timestamp,
            FromUserId = message.SenderId,
            FromFullName = message.Sender.FirstName + " " + message.Sender.LastName,
            RoomId = message.ChatRoomId
        };
        response = Response<MessageView>.Builder().SetData(messageView).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
    [HttpGet("chat-room/{chatRoomId}")]
    public async Task<ActionResult<Response<List<MessageView>>>> GetMessageByRoomId(Guid chatRoomId)
    {

        var messagesList = await _messageRepository.WhereAsync(x => x.ChatRoomId == chatRoomId, "Sender");
        if (messagesList == null)
        {
            return Response<List<MessageView>>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        messagesList = messagesList.OrderByDescending(x => x.Timestamp).ToList();
        var messageViewList = new List<MessageView>();
        foreach (var message in messagesList)
        {
            var messageView = new MessageView()
            {
                Content = message.Content,
                Timestamp = message.Timestamp,
                FromUserId = message.SenderId,
                FromFullName = message.Sender.FirstName + " " + message.Sender.LastName,
                RoomId = message.ChatRoomId
            };
            messageViewList.Add(messageView);
        }

        var response = Response<List<MessageView>>.Builder().SetData(messageViewList).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        return StatusCode((int)HttpStatusCode.OK, response);
    }
    [HttpGet()]
    public async Task<ActionResult<List<MessageView>>> GetMessages([FromQuery] Guid referenceId, [FromQuery] TypeChatRoom type)
    {
        var chatRoom = await _chatRoomRepository.FindWithCondition(x => x.ReferenceId == referenceId && x.TypeChatRoom == type);
        if (chatRoom == null)
        {
            return Response<List<MessageView>>.Builder().SetMessage("ReferenceId not found").SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound);

        }
        var messagesList = await _messageRepository.WhereAsync(x => x.ChatRoomId == chatRoom.Id, "Sender");
        if (messagesList == null)
        {
            return Response<List<MessageView>>.Builder().SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        messagesList = messagesList.OrderByDescending(x => x.Timestamp).ToList();
        var messageViewList = new List<MessageView>();
        foreach (var message in messagesList)
        {
            var messageView = new MessageView()
            {
                Content = message.Content,
                Timestamp = message.Timestamp,
                FromUserId = message.SenderId,
                FromFullName = message.Sender.FirstName + " " + message.Sender.LastName,
                RoomId = message.ChatRoomId
            };
            messageViewList.Add(messageView);
        }

        var response = Response<List<MessageView>>.Builder().SetData(messageViewList).SetMessage("Success").SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
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