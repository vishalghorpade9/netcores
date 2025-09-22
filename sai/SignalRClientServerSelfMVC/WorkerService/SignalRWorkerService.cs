
using Microsoft.AspNetCore.SignalR;
using SignalRClientServerSelfMVC.Models;

namespace SignalRClientServerSelfMVC.WorkerService
{
    public class SignalRWorkerService : BackgroundService
    {
        private readonly ILogger<SignalRWorkerService> logger;
        private readonly IHubContext<ChatHub> hubContext;

        public SignalRWorkerService(ILogger<SignalRWorkerService> logger, IHubContext<ChatHub> hubContext)
        {
            this.logger = logger;
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Producer worker running at " + DateTime.Now);
                var message = "Producer worker running at " + DateTime.Now;

                // Send a message to all connected clients
                await hubContext.Clients.All.SendAsync("ReceiveMessage", message);

                // await Task.Delay(Timeout.Infinite, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
