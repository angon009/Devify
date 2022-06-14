using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.SuperAdmin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Payments()
        {
            return View();
        }
        public IActionResult Stores()
        {
            return View();
        }
    }
}
