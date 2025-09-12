
using RabbitMQ.Client;
using System.Text;

namespace WorkerServiceMicroservice
{
    public class ProducerWorkerService : BackgroundService
    {
        readonly ILogger<ProducerWorkerService> _logger;

        public ProducerWorkerService(ILogger<ProducerWorkerService> logger)
        {
            _logger = logger;
        }

        // rabbitmq
        public async void SendProductMessage(string message)
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "mes-queue", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            //const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "mes-queue", body: body);            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Producer worker running at " + DateTime.Now);
                SendProductMessage("Producer worker running at " + DateTime.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
