using Autofac;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Utility;
using ECommerce.Web.Areas.Vendor.Models.SalesModels;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    public class SalesController : Controller
    {
        private readonly ILifetimeScope _scope;
        ILogger<SalesController> _logger;

        public SalesController(ILifetimeScope scope, ILogger<SalesController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = _scope.Resolve<SalesViewModel>();
            ViewBag.Store = TempData.Peek<StoreDetailsViewModel>("StoreInfo");
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            await model.GetCountAsync(storeId);
            return View(model);
        }

        public async Task<JsonResult> TotalSales(DateTime from, DateTime to)
       {
            var model = _scope.Resolve<SalesViewModel>();
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            model.FromDate = from;
            model.ToDate = to;
            var data = await model.GetAllSalesAsync(dataTableModel, storeId);
            return Json(data);
        }

        public async Task<IActionResult> GetSales(SalesViewModel search)
        {
            var model = _scope.Resolve<SalesViewModel>();
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            model.FromDate = search.FromDate;
            model.ToDate = search.ToDate;
            await model.GetCountAsync(storeId);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            return RedirectToAction("Detail", "Orders", new { id = id, from = "sales" });
        }
    }
}
