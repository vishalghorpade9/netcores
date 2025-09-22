using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRClientServerSelfMVC.Models;
using System.Diagnostics;

namespace SignalRClientServerSelfMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ChatHub> hubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult DynamicMessageToSpecificClients(string clientName)
        {
            TempData["ClientName"] = clientName;
            return View();
        }

        
        public IActionResult OpcUaSignalRDashboard()
        {
            return View();
        }

        
    }
}
