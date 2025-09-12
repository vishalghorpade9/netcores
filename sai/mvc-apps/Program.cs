using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mvc_apps;
using mvc_apps.Data;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(action => {
    var connection = "localhost:6379,password=redisPassword"; //the redis connection
    action.Configuration = connection;
});

// Serilog configurations
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();

//var conn = builder.Configuration.GetConnectionString("DefaultConnections");

//var logger = new LoggerConfiguration()
//            .MinimumLevel.Information()
//            .WriteTo.Console()
//            .WriteTo.MSSqlServer(conn, new MSSqlServerSinkOptions { TableName = "ErrorLogs", AutoCreateSqlTable = true }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
//            .WriteTo.MSSqlServer(conn, new MSSqlServerSinkOptions { TableName = "InfoLogs", AutoCreateSqlTable = true }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
//            .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnectionString"));
});

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});



builder.Services.AddScoped<ISharedViewLocalizer, SharedViewLocalizer>();

// Add services to the container.
//builder.Services.AddControllersWithViews().AddMvcLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
//    .AddDataAnnotationsLocalization();

builder.Services.AddControllersWithViews().AddMvcLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);

            return factory.Create("SharedResources", assemblyName.Name);
        };
    });

builder.Services.AddMvc()
                .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>
(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("es"),
            new CultureInfo("fr")
        };
        options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    }
);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//var supportedCultures = new[] { "en", "fr", "es" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);


//app.UseRequestLocalization(localizationOptions);

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



//using Microsoft.EntityFrameworkCore;
//using mvc_apps.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

///************** SESSION START *******************/
//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(60);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
///************** SESSION END *******************/

//builder.Services.AddDbContext<DatabaseContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnectionString"));
//});


//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.UseSession();   // SESSION

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
