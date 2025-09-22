using Microsoft.EntityFrameworkCore;
using opc_ua_worker_service;
using opc_ua_worker_service.Data;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();
builder.Services.AddHostedService<OpCUaWorkers>();

//builder.Services.AddDbContext<DatabaseContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
//});

var host = builder.Build();
host.Run();
