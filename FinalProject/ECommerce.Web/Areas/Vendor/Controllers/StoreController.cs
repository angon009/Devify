using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.StoreModels;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.StoreAdmin.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    [Authorize]
    public class StoreController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<StoreController> _logger;

        public StoreController(ILifetimeScope scope, ILogger<StoreController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Getstores()
        {
            try
            {
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<StoreListViewModel>();
                var data = await model.GetPagedStores(dataTableModel);
                var jsonData = Json(data);
                return jsonData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }

        //Store Details
        public IActionResult Details(int id)
        {
            return View();
        }

        //Edit Store
        public IActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(StoreUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Resolve(_scope);
                    await model.UpdateStore();
                    var modelTemp = _scope.Resolve<StoreDetailsViewModel>();
                    modelTemp.Resolve(_scope);
                    StoreDetailsViewModel passData = await modelTemp.GetStoreDetails(model.Id);
                    TempData.Put<StoreDetailsViewModel>("StoreInfo", passData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }
        //Change Store Status to Inactive
        public async Task<IActionResult> Deactivate(int id)
        {
            StoreStatusChangeModel storeStatusChangeModel = _scope.Resolve<StoreStatusChangeModel>();
            if (ModelState.IsValid)
            {  
                await storeStatusChangeModel.DisableStoreAsync(id);
            }
            return RedirectToAction("Index");
        }
        //Change Store Status to Active
        public async Task<IActionResult> Activate(int id)
        {
            StoreStatusChangeModel storeStatusChangeModel = _scope.Resolve<StoreStatusChangeModel>();
            if (ModelState.IsValid)
            {  
                await storeStatusChangeModel.EnableStoreAsync(id);
            }
            return RedirectToAction("Index");
        }

        //Change Store Status to OnDelete
        public async Task<IActionResult> OnDelete(int id)
        {
            StoreStatusChangeModel storeStatusChangeModel = _scope.Resolve<StoreStatusChangeModel>();
            if (ModelState.IsValid)
            {
                await storeStatusChangeModel.OnDeleteStoreAsync(id);
            }
            return RedirectToAction("Index"); 
        }
    }
}