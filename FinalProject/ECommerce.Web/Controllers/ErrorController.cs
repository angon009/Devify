using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
