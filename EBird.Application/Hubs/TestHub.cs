namespace EBird.Application.Hubs;

using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
public class TestHub : Hub {
    public async Task SendMessage(string message,WebSocket ws)
        {
            await Clients.All.SendAsync("RECEIVE_MSG", message);
            await Clients.All.SendAsync("MSG", message);
            await Clients.All.SendAsync("MSG2", message);
        }
}