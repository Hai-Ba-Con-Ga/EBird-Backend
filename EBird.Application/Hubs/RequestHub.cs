using Microsoft.AspNetCore.SignalR;

namespace EBird.Application.Hubs
{
    public class RequestHub : Hub
    {
        public async Task SendRequest(string message)
        {
            await Clients.All.SendAsync("ReceiveRequest", message);
        }
    }
}