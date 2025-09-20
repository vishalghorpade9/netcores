using MicroSignalRHubs.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Define a policy name

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("https://localhost:7036", // Replace with your allowed origins
//                                             "http://www.contoso.com")
//                                .AllowAnyHeader()
//                                .AllowAnyMethod();
//                      });
//});

builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

builder.Services.AddControllers();

builder.Services.AddSignalR();

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

app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

//app.UseEndpoints(endpoints => {
//    endpoints.MapControllers();
//    endpoints.MapHub<ChatHub>("/chatHub");
//});

app.MapControllers();


// app.MapHub<ChatHub>("/chatHub");


app.Run();
