using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using windows_authentication_examples.Data;
using windows_authentication_examples.Models;

namespace windows_authentication_examples.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dataContext = dataContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            try
            {


                string? userName = this.httpContextAccessor.HttpContext?.User?.Identity?.Name;
                if (!userName.IsNullOrEmpty())
                {
                    // process to check user in database
                    User? user = dataContext.Users.Where(p => p.UserName == userName && p.IsActive == true).FirstOrDefault();
                    if (user != null)
                    {
                        // no user found or user account is in-active
                        HttpContext.Session.SetString("UserSessionDetails", JsonSerializer.Serialize(user));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // user account found and active
                        // set session variable
                        TempData["UserName"] = userName;
                        return View("LoginError");
                    }
                }
                else
                {
                    // no user login
                    return View("NoUserLogin");

                }
            }
            catch (Exception ex)
            {
                TempData["UserName"] = ex.ToString();
                return View("LoginError");
            }
        }
    }
}
