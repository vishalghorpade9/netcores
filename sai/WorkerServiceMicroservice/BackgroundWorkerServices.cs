
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
public class BackgroundWorkerServices : BackgroundService
{
    readonly ILogger<BackgroundWorkerServices> _logger;

    public BackgroundWorkerServices(ILogger<BackgroundWorkerServices> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopped");
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {        
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at " + DateTime.Now);            
            await Task.Delay(1000, stoppingToken);
        }
    }

    

}