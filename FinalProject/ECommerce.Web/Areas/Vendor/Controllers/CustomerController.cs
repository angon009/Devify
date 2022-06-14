using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.CustomerModels;
using ECommerce.Web.ControllerLevelValidation;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    public class CustomerController : Controller
    {
        private ILifetimeScope _scope;
        private ILogger<CustomerController> _logger;
        public CustomerController(ILifetimeScope scope, ILogger<CustomerController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetCustomers()
        {
            var model = _scope.Resolve<CustomerListViewModel>();
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            var result = await model.GetCustomersAsync(storeId);

            return Json(result);
        }
    }
}
