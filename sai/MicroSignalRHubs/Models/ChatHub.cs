using Microsoft.AspNetCore.SignalR;

namespace MicroSignalRHubs.Models
{
    public class ChatHub : Hub
    {
        private readonly IHubContext<ChatHub> context;

        public ChatHub(IHubContext<ChatHub> context)
        {
            this.context = context;
        }

        public async Task SendMessage(string machineName, string user, string message)
        {
            await context.Clients.All.SendAsync(machineName, user, message);
        }
    }
}
