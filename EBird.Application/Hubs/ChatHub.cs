using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Chat;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace EBird.Application.Hubs;
public class ChatHub : Hub
{
    private readonly static string _Connections = "Connections";
    private readonly static Dictionary<string, UserConnection> _ConnectionsMap = new Dictionary<string, UserConnection>();
    // public ChatHub(Dictionary<string, UserConnection> ConnectionsMap)
    // {
    //     _Connections = "Connections";
    //     _ConnectionsMap = ConnectionsMap;
    // }
    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_ConnectionsMap.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            _ConnectionsMap.Remove(Context.ConnectionId);
            Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", $"{userConnection.UserId} has left");
            SendUsersConnected(userConnection.RoomId);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public Task SendUsersConnected(string roomId)
    {
        var users = _ConnectionsMap.Values
        .Where(c => c.RoomId == roomId)
            .Select(c => c.UserId);

        return Clients.Group(roomId).SendAsync("UsersInRoom", users);
    }
    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomId);

        _ConnectionsMap[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", $"{userConnection.UserId} has joined {userConnection.RoomId}");

        await SendUsersConnected(userConnection.RoomId);
    }
    public async Task SendMessage(string message)
    {
        if (_ConnectionsMap.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", userConnection.UserId, message);
        }
    }







}