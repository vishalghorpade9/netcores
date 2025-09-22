
using Microsoft.AspNetCore.SignalR;
using SignalRClientServerSelfMVC.Models;

namespace SignalRClientServerSelfMVC.WorkerService
{
    public class SignalRWorkerService : BackgroundService
    {
        private readonly ILogger<SignalRWorkerService> logger;
        private readonly IHubContext<ChatHub> hubContext;

        private readonly string[] clientName = ["station1", "station2", "station3", "station4", "station5"];

        public SignalRWorkerService(ILogger<SignalRWorkerService> logger, IHubContext<ChatHub> hubContext)
        {
            this.logger = logger;
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            while (!stoppingToken.IsCancellationRequested)
            {
                // logger.LogInformation("Producer worker running at " + DateTime.Now);
                var message = "Producer worker running at " + DateTime.Now;

                // Send a message to all connected clients
                // await hubContext.Clients.All.SendAsync("ReceiveMessage", message);

                int randomNumber = new Random().Next(0, 4);
                string brodcastToclientName = clientName[randomNumber];
                message = "Message for " + brodcastToclientName + " from server, how are you? " + DateTime.Now.ToString();
                await hubContext.Clients.All.SendAsync(brodcastToclientName, message);

                // await Task.Delay(Timeout.Infinite, stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
