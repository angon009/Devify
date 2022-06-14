using Autofac;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.ControllerLevelValidation;

namespace ECommerce.Web.Areas.StoreAdmin.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    public class CategoryController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<CategoryController> _logger;

        public CategoryController(ILifetimeScope scope, ILogger<CategoryController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Currently in Category Index action method");
            return View();
        }

        public async Task<JsonResult> Getcategories()
        {
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id; //Store Id will get from session
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<CategoryListViewModel>();
            var list = await model.GetPagedCategoriesAsync(dataTableModel, storeId);
            return Json(list);
        }

        //Category Create
        public IActionResult Create()
        {
            var model = _scope.Resolve<CategoryCreateViewModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            model.StoreId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id; // Store Id from Session
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.CreateCategoryAsync();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully added a new Category.",
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
                        Message = "There was a problem in creating Category.",
                        Type = ResponseTypes.Warning
                    });
                }
            }

            return View(model);
        }

        //Cateegory Details
        public IActionResult Details(int id)
        {
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "SubCategory", new { id = id });
        }

        //Edit Cateegory
        public async Task<IActionResult> Edit(int id)
        {
            var model = _scope.Resolve<CategoryCreateViewModel>();
            await model.LoadData(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.EditCategory();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully Updated the Category.",
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
                        Message = "There was a problem in Updating the Category.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            return View(model);
        }

        //Delete Cateegory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = _scope.Resolve<CategoryDeleteModel>();
                await model.DeleteCategory(id);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfuly Deleted the Category.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in deleteing the Category.",
                    Type = ResponseTypes.Warning
                });
            }
            return RedirectToAction("Index");
        }
    }
}
