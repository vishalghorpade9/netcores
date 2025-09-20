using MicroSignalRHubs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MicroSignalRHubs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineStatusController : ControllerBase
    {
        private readonly IHubContext<ChatHub> context;

        public MachineStatusController(IHubContext<ChatHub> context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMachineStatus(string machineName)
        {
            int randomNumber = new Random().Next(0, 3);
            await new ChatHub(context).SendMessage("ReceiveMessage", "user", randomNumber.ToString());
            return Ok();
        }
    }
}
