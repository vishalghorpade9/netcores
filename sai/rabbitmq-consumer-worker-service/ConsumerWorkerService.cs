using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rabbitmq_consumer_worker_service
{
    public class ConsumerWorkerService : BackgroundService
    {
        private readonly ILogger<ConsumerWorkerService> logger;
        private static string logFilePath = "C:\\Users\\VGhorpade\\source\\repos\\netcores\\sai\\rabbitmq-consumer-worker-service\\application_log.txt";
        public ConsumerWorkerService(ILogger<ConsumerWorkerService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            createLogs("Services started");
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Producer worker running at " + DateTime.Now);
                // SendProductMessage("Producer worker running at " + DateTime.Now);
                configureRabbitMQReceiver();
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public async void configureRabbitMQReceiver()
        {
            createLogs("Inside configuration");
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "mes-queue", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");
            createLogs(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                createLogs($" [x] Received {message}");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("mes-queue", autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            createLogs("function end");
        }

        void createLogs(string log)
        {
            // Ensure the directory exists (optional, if you're not using a specific subfolder)
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Format the log entry with a timestamp
            string logEntry = $"{DateTime.Now}: {log}";

            // Append the log entry to the file. If the file doesn't exist, it will be created.
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}
