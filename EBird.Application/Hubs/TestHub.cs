namespace EBird.Application.Hubs;
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
using System.Net.WebSockets;
public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
}
public class TestHub : Hub
{
    private readonly static string _Connections = "Connections";


    private readonly IGenericRepository<ChatRoomEntity> _chatRoomRepository;
    private readonly IGenericRepository<AccountEntity> _accountRepository;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly IGenericRepository<ParticipantEntity> _participantRepository;
    private readonly IMapper _mapper;
    private readonly IAuthenticationServices _authenticationServices;



    public TestHub(IGenericRepository<ChatRoomEntity> chatRoomRepository, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<MessageEntity> messageRepository, IGenericRepository<ParticipantEntity> participantRepository, IMapper mapper, IAuthenticationServices authenticationServices)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
        _authenticationServices = authenticationServices;
        _mapper = mapper;
    }
    // public async Task SendMessage(string message, Student? student)
    // {
    //     await Clients.All.SendAsync("RECEIVE_MSG", message, student);
    //     await Clients.All.SendAsync("MSG", student.Name);
    //     await Clients.All.SendAsync("MSG2", student.Age);
    // }
    public override async Task OnConnectedAsync()
    {
        try
        {
            var chatRoomIdRaw = Context.GetHttpContext().Request.Query["chatRoomId"].ToString().ToLower();
            var userIdRaw = Context.GetHttpContext().Request.Query["userId"].ToString().ToLower();
            Console.WriteLine("===================");

            Console.WriteLine("userIdRaw: " + userIdRaw);
            Console.WriteLine("chatRoomIdRaw: " + chatRoomIdRaw);

            // var userId = Context.User.GetUserId();
            Context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userIdRaw) }));
            var userId = Guid.Parse(userIdRaw);
            var chatRoomId = Guid.Parse(chatRoomIdRaw);
            Console.WriteLine("===================");

            var account = await _accountRepository.GetByIdActiveAsync(userId);

            await Clients.Group(chatRoomIdRaw)
            .SendAsync(HubEvents.UserActive, $"{account.FirstName} {account.LastName} has joined {chatRoomId}");
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public async Task SendMessage(string message, string chatRoomId)
    {
        try
        {
            var userId = Context.User.GetUserId();
            var account = await _accountRepository.GetByIdActiveAsync(userId);
            await Clients.Group(chatRoomId.ToString()).SendAsync(HubEvents.NewMessage, account, message);
            // await Clients.All.SendAsync(HubEvents.NewMessage, account, message);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}