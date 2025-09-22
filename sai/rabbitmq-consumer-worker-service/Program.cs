using rabbitmq_consumer_worker_service;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();
builder.Services.AddHostedService<ConsumerWorkerService>();

var host = builder.Build();
host.Run();
