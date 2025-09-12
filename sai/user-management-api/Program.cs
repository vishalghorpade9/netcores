using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using user_management_api.Data;
using user_management_api.Mapping;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// database registration
builder.Services.AddDbContext<UserManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementConnectionString"));
});

// auto profile mapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
