using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
