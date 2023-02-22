namespace EBird.Application.Hubs;

using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
public class Student {
  public string Name { get; set; }
  public int Age { get; set; }
}
public class TestHub : Hub
{
  public async Task SendMessage(string message,Student? student)
  {
    await Clients.All.SendAsync("RECEIVE_MSG", message,student);
    await Clients.All.SendAsync("MSG", student.Name);
    await Clients.All.SendAsync("MSG2", student.Age);
  }
}