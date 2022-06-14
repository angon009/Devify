using Autofac;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.ForDiscount;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    [Authorize]
    public class DiscountController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<DiscountController> _logger;

        public DiscountController(ILifetimeScope scope, ILogger<DiscountController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Currently in DiscountController Index method.");
            
            return View();
        }

        public async Task<JsonResult> GetDiscounts(int id)
        {
            try
            {
                int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<DiscountListViewModel>();
                var list = await model.GetPagedDiscounts(dataTableModel, storeId);
                return Json(list);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Json(null);
        }

        //Create Discount
        public IActionResult Create()
        {
            var model = _scope.Resolve<DiscountCreateModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCreateModel model)
        {
            model.StoreId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.CreateDiscountAsync();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully added a new Sub-Category.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (DuplicateDataException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Warning
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in creating Discount.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            return View(model);
        }

        //Discount Details
        public async Task<IActionResult> Details(int id)
        {
            var model = _scope.Resolve<DiscountUpdateModel>();
            var data=await model.LoadDataAsync(id);
            return View(data);
        }

        //Update Discount
        public async Task<IActionResult> Update(int id)
        {
            var model = _scope.Resolve<DiscountUpdateModel>();
            var data=await model.LoadDataAsync(id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DiscountUpdateModel model)
        {
            model.StoreId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.UpdateDiscountAsync();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully Updated the Discount.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (DuplicateDataException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Warning
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in Updating the Sub-Category.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            return View(model);
        }

        //Delete Discount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = _scope.Resolve<DiscountListViewModel>();
                await model.DeleteDiscount(id);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfuly Deleted the Sub-Category.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in deleteing the Discount.",
                    Type = ResponseTypes.Warning
                });
            }
            return RedirectToAction("Index");
        }
    }
}