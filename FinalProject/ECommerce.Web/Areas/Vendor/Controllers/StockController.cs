using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.ProductModels;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.StockModels;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    public class StockController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<StockController> _logger;
        public StockController(ILifetimeScope scope, ILogger<StockController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Update(int id,int quantity)
        {
            try
            {
                StockUpdateModel stockUpdateModel = _scope.Resolve<StockUpdateModel>();
                if (ModelState.IsValid)
                {
                    await stockUpdateModel.UpdateQuantityAsync(id, quantity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public async Task<JsonResult> GetStocks()
        {
            try
            {
                int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                //Store Id will get from session
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<StockListViewModel>();
                var list = await model.GetPagedStores(dataTableModel, storeId);
                return Json(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }
    }
}
