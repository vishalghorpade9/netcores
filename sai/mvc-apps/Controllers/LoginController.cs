using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mvc_apps.Data;
using mvc_apps.Models;
using System.Text.Json;

namespace mvc_apps.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            string? userDetails = HttpContext.Session.GetString("UserSessionDetails");
            if (!userDetails.IsNullOrEmpty())
            {
                // process to deserialize
                ShopUser? shopUser=  JsonSerializer.Deserialize<ShopUser>(userDetails);
                if (shopUser != null)
                {
                    // user already login and trying to access login page
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult ValidateUserLogin(ShopUser shopUser)
        {
            string userName = shopUser.UserName.Trim(), password = shopUser.Password.Trim();
            ShopUser? user = databaseContext.ShopUsers.Where(p => p.UserName == userName && p.Password == password && p.IsActive==true).FirstOrDefault();
            if (user != null)
            {
                // process to save session data
                HttpContext.Session.SetString("UserSessionDetails", JsonSerializer.Serialize(user));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // process to add error and return to Index
                TempData["Error"] = "Invalid username or password";
            }
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
