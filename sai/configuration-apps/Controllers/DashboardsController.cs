using Microsoft.AspNetCore.Mvc;

namespace configuration_apps.Controllers
{
    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MachineLayout()
        {
            return View();
        }
    }
}
