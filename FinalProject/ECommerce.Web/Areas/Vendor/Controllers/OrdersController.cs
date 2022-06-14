using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.OrdersModels;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<OrdersController> _logger;

        public OrdersController(ILifetimeScope scope, ILogger<OrdersController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetOrders()
        {
            try
            {
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<OrderListViewModel>();
                var storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                return Json(await model.GetOrdersAsync(dataTableModel, storeId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }
        public async Task<IActionResult> Detail(int id, string from)
        {
            var model = _scope.Resolve<OrderListViewModel>();
            try
            {
                model.From = from;
                await model.GetOrderAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(OrderListViewModel model)
        {
            try
            {
                model.Resolve(_scope);
                await model.ChangeStatusAsync();
                await model.GetOrderAsync(model.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }
    }
}