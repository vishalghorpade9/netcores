using Microsoft.AspNetCore.Mvc;

namespace configuration_apps.Controllers
{
    public class StarterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
