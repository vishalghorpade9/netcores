using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using mvc_apps.Models;
using System.Diagnostics;

namespace mvc_apps.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        // private readonly IHtmlLocalizer<HomeController> htmlLocalizer;

        public HomeController(ILogger<HomeController> logger
            //, IHtmlLocalizer<HomeController> htmlLocalizer
            )
        {
            _logger = logger;
            // this.htmlLocalizer = htmlLocalizer;
        }

        public IActionResult Index()
        {
            // var test = htmlLocalizer["EquipmentName"];
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

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(3) });
            // return RedirectToAction(nameof(Index));
            return LocalRedirect(returnUrl);
        }
    }
}
