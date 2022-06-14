using Autofac;
using AutoMapper;
using ECommerce.Membership.Services;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.ProductModels;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Web.Areas.StoreAdmin.Controllers
{
    [Area("Vendor")]
    [Authorize]
    public class DashboardController : Controller
    {
        private ILifetimeScope _scope;
        ILogger<DashboardController> _logger;
        private readonly UserManager _userManger;
        private IMapper _mapper;
        public DashboardController(ILifetimeScope scope, ILogger<DashboardController> logger, UserManager userManager, IMapper mapper)
        {
            _mapper = mapper;
            _scope = scope;
            _logger = logger;
            _userManger = userManager;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if(Id == 0)
            {
                try
                {
                    if (TempData.Peek<StoreDetailsViewModel>("StoreInfo") != null)
                    {
                        Id = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                    };
                    if (Id == 0)
                    {
                        return RedirectToAction("Stores");
                    }
                }
                catch(Exception e)
                {

                    _logger.LogError(e.Message);
                }
            }
            var InventoryModel = _scope.Resolve<InventoryAlertCountModel>();
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            ViewBag.AlertData=await InventoryModel.GetMinAndOutStockCount(dataTableModel, Id);
            ViewBag.TotalData = await InventoryModel.GetDashboardValues(Id);
            var model = _scope.Resolve<StoreDetailsViewModel>();
            ViewBag.Products = await model.GetLatestOrders(Id);
            //model.Resolve(_scope);
            StoreDetailsViewModel passData = await model.GetStoreDetails(Id);
            TempData.Put<StoreDetailsViewModel>("StoreInfo", passData);
            return View();
        }

        [Route("Stores")]
        public async Task<IActionResult> Stores()
        {
            try
            {
                StoreListViewModel model = _scope.Resolve<StoreListViewModel>();

                model.ApplicationUser = await _userManger.GetUserAsync(HttpContext.User);
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var list = await model.GetStoresByUserId(userId);
                ViewBag.list = list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(StoreCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Resolve(_scope);
                    model.ApplicationUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await model.CreateStore();
                    return RedirectToAction("Stores");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return RedirectToAction("Stores");
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
