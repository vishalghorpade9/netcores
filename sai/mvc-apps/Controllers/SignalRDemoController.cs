using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using mvc_apps.Models;

namespace mvc_apps.Controllers
{
    public class SignalRDemoController : Controller
    {
        private readonly IHubContext<ChatHub> hubContext;

        public SignalRDemoController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MachineStatusUsingSignalR(string machineName)
        {
            TempData["MachineName"]=machineName;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> sendMachineStatusUsingSignalR(string machineName)
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage","user","1");
            return View();
        }
    }
}
