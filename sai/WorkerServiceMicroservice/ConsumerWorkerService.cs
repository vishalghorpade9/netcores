
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkerServiceMicroservice
{
    public class ConsumerWorkerService : BackgroundService
    {
        private readonly ILogger<ConsumerWorkerService> logger;

        public ConsumerWorkerService(ILogger<ConsumerWorkerService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Consumer will read data npw " + DateTime.Now);
                ReadQueue();
                await Task.Delay(30000, stoppingToken);
            }

        }

        public async void ReadQueue()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "mes-queue", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            logger.LogInformation(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                logger.LogInformation($"Consumer [x] body {ea.Body.ToArray().Count()}");
                var message = Encoding.UTF8.GetString(body);
                // Console.WriteLine($" [x] Received {message}");
                logger.LogInformation($"Consumer [x] Received {message}");
                return Task.CompletedTask;

                // Acknowledge the message to remove it from the queue
                // channel.BasicAck(ea.DeliveryTag, multiple: false);
            };          

            await channel.BasicConsumeAsync("mes-queue", autoAck: true, consumer: consumer);
           
        }       
    }
}
