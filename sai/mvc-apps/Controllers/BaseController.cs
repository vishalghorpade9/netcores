using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace mvc_apps.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the "UserID" session variable exists, indicating a logged-in user
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserSessionDetails")))
            {
                // If not logged in, redirect to the login page
                filterContext.Result = new RedirectToActionResult("Index", "Login",  null);
            }

            base.OnActionExecuting(filterContext);
        }
        
    }
}
