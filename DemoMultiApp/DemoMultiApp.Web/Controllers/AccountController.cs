using Microsoft.AspNetCore.Mvc;

namespace DemoMultiApp.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
