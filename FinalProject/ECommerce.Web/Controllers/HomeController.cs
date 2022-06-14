using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForStock;
using ECommerce.Utility;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.Web.Controllers
{
    [ServiceFilter(typeof(StoreSubDomainChecker))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStockService _stockService;
        public IActionResult message()
        {
            return View();
        }

        public HomeController(ILogger<HomeController> logger, IStockService stockService)
        {
            _logger = logger;
            _stockService = stockService;

        }

        public IActionResult Index()
        {

            var subDomain = UrlAction.GetSubDomain();
            if (subDomain != null)
                return RedirectToAction("Index", "Store");

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
              
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}