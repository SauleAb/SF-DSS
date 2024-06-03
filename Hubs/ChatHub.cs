using Microsoft.AspNetCore.SignalR;

namespace SF_DSS
{
    public class ChatHub : Hub
    {
            public async Task SendMessage(string message)
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
    }
}
