using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Areas.Vendor.Models.ProductModels;
using Autofac;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Utility;
using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    public class InventoryController : Controller
    {
        private ILifetimeScope _scope;

        public InventoryController(ILifetimeScope scope)
        {
            _scope = scope;


        }
        public async Task<IActionResult> Update()
        {
            var model=_scope.Resolve<InventoryAlertCountModel>();
            var storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            model.StoreId = storeId;
            var passModel=await model.GetInventorybyStoreId();
            
            if (passModel != null)
            {
                passModel.isActive = passModel.status == "Active";
            }
            else
            {
                passModel.isActive = false;
            }
            return View(passModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(InventoryAlertCountModel model)
        {
            try
            {
                model.Resolve(_scope);
                model.status = model.isActive ? "Active" : "Inactive";
                await model.CreateOrUpdateInventoryAlert();
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Type = ResponseTypes.Success,
                    Message = "Successfully updated your inventory minimum product!"
                });
            }
            catch(Exception ex)
            {
                TempData.Put<ResponseModel>("ResponseMessage",new ResponseModel{
                    Type=ResponseTypes.Warning,
                    Message="Failed to save please try again!"
                });
            }
            
            return View(model);
        }
        public async Task<IActionResult> OutOfStock()
        {
            return View();
        }
        public async Task<JsonResult> GetOutOfStockProducts()
        {
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id; //Store Id will get from session
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<InventoryAlertCountModel>();
            var list = await model.GetOutOfStockProducts(dataTableModel, storeId);
            return Json(list);
        }
        public async Task<IActionResult> RunningOut()
        {
            return View();
        }
        public async Task<JsonResult> RunningOutOfStock()
        {
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id; //Store Id will get from session
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<InventoryAlertCountModel>();
            var list = await model.GetRunningOutOfStockProducts(dataTableModel, storeId);
            return Json(list);
        }
    }
}
