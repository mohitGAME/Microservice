using Microsoft.AspNetCore.Mvc;

namespace Mango.GatewaySolution.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
