using Microsoft.AspNetCore.Mvc;

namespace easyPC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
