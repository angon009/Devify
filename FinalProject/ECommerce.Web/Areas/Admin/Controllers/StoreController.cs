using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.Admin.Models;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.SuperAdmin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<StoreController> _logger;

        public StoreController(ILogger<StoreController> logger, ILifetimeScope scope)
        {
            _scope = scope;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Payments()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = _scope.Resolve<StoreDetailsViewModel>();
            try
            {
                await model.GetStoreDetails(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }
        public JsonResult GetStores()
        {
            var dataTableAjaxRequestModel = new DataTablesAjaxRequestModel(Request);
            var storeListModel = _scope.Resolve<StoreListModel>();
            return Json(storeListModel.GetPagedStores(dataTableAjaxRequestModel));
        }

        public async Task<JsonResult> GetStoresPayment()
        {
            var dataTableAjaxRequestModel = new DataTablesAjaxRequestModel(Request);
            var paymentListModel = _scope.Resolve<PaymentListModel>();
            return Json(await paymentListModel.GetPagedStoresPayment(dataTableAjaxRequestModel));
        }

        public async Task<IActionResult> Block(int id)
        {
            StoreStatusModel storestatusModel = new StoreStatusModel();
            if (ModelState.IsValid)
            {
                try
                {
                    storestatusModel.Resolve(_scope);
                    await storestatusModel.DisableStoreAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Allow(int id)
        {
            StoreStatusModel storestatusModel = new StoreStatusModel();
            if (ModelState.IsValid)
            {
                try
                {
                    storestatusModel.Resolve(_scope);
                    await storestatusModel.EnableStoreAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = _scope.Resolve<StoreStatusModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    await model.DeleteStoreAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }
    }
}