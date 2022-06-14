using Autofac;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.StoreAdmin.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    public class SubCategoryController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<SubCategoryController> _logger;

        public SubCategoryController(ILifetimeScope scope, ILogger<SubCategoryController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int id)
        {
            _logger.LogInformation("Currently in Sub-Category Index method.");
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            TempData.Put<CategoryModel>("CategoryId", new CategoryModel { Id = id });
            var model = _scope.Resolve<SubCategoryListViewModel>();
            ViewBag.Category = await model.GetCategory(id);
            return View();
        }

        public async Task<JsonResult> Getsubcategories(int id)
        {
            try
            {
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<SubCategoryListViewModel>();
                var list = await model.GetPagedSubCategories(dataTableModel, id);
                return Json(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }

        //Create Sub-Category
        public IActionResult Create()
        {
            var model = _scope.Resolve<SubCategoryCreateModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryCreateModel model)
        {
            model.CategoryId = TempData.Peek<CategoryModel>("CategoryId").Id; //get from Session
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.CreateSubCategoryAsync();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully added a new Sub-Category.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index", new { id = model.CategoryId });
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
                        Message = "There was a problem in creating Sub-Category.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            return View(model);
        }

        //sub-Category Details
        public IActionResult Details(int id)
        {
            return View();
        }

        //Edit sub-Category
        public async Task<IActionResult> Edit(int id)
        {
            var model = _scope.Resolve<SubCategoryCreateModel>();
            try
            {
                await model.LoadData(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                try
                {
                    await model.EditSubCategory();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully Updated the Sub-Category.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index", new { id = model.CategoryId });
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

        //Delete sub-Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = _scope.Resolve<SubCategoryDeleteModel>();
                await model.DeleteSubCategory(id);
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
                    Message = "There was a problem in deleteing the Sub-Category.",
                    Type = ResponseTypes.Warning
                });
            }
            var categoryId = TempData.Peek<CategoryModel>("CategoryId").Id;
            return RedirectToAction("Index", new { id = categoryId });
        }
    }
}