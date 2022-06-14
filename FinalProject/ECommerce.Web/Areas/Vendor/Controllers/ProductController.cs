using Autofac;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.StoreAdmin.Models.ProductModels;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels;
using ECommerce.Web.Areas.Vendor.Models.ForDiscount;
using ECommerce.Web.Areas.Vendor.Models.ProductModels;
using ECommerce.Web.ControllerLevelValidation;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.StoreAdmin.Controllers
{
    [StoreIdChecker]
    [Area("Vendor")]
    public class ProductController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILifetimeScope scope, ILogger<ProductController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public async Task<IActionResult> DiscountAssign()
        {
            var model = _scope.Resolve<ProductListViewModel>();
            var discountModel = _scope.Resolve<DiscountListViewModel>();
            
            int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            model.discountList = await discountModel.GetDiscountsAsync(storeId);
            await model.LoadDataToProducts(storeId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DiscountAssign(ProductListViewModel model)
        {
            try
            {
                model.Resolve(_scope);
                await model.AssignDiscounttoProducts();
                return RedirectToAction("DiscountAssign");
            }
            catch
            {

            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Getproducts()
        {
            try
            {
                int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                //Store Id will get from session
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<ProductListViewModel>();
                var list = await model.GetPagedStores(dataTableModel, storeId);
                return Json(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }

        //Pruduct Create
        public async Task<IActionResult> Create()
        {
            var model = _scope.Resolve<ProductCreateViewModel>();
            var categoryModel = _scope.Resolve<CategoryListViewModel>();
            var discountModel = _scope.Resolve<DiscountListViewModel>();
            var storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            try
            {
                model.categoryList = await categoryModel.GetCategoriesAsync(storeId);
                model.discountList = await discountModel.GetDiscountsAsync(storeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel productModel)
        {
            productModel.Resolve(_scope);
            var categoryModel = _scope.Resolve<CategoryListViewModel>();
            var discountModel=_scope.Resolve<DiscountListViewModel>();
            productModel.StoreId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            productModel.categoryList = await categoryModel.GetCategoriesAsync(productModel.StoreId);
            productModel.discountList = await discountModel.GetDiscountsAsync(productModel.StoreId);
       
            if (ModelState.IsValid)
            {
                try
                {
                    await productModel.CreateProductAsync();
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully added a new Product.",
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
                        Message = "There was a problem in creating Product.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            return View(productModel);
        }
        //Update Product
        public async Task<IActionResult> Update(int id)
        {
            var model = _scope.Resolve<ProductUpdateViewModel>();
            await model.LoadData(id);
            model.CategoryId = (int)model.SubCatetory!.CategoryId!;
            var categoryModel = _scope.Resolve<CategoryListViewModel>();
            var discountModel = _scope.Resolve<DiscountListViewModel>();
            var storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            try
            {
                model.discountList = await discountModel.GetDiscountsAsync(storeId);
                model.categoryList = await categoryModel.GetCategoriesAsync(storeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateViewModel productModel)
        {
            var categoryModel = _scope.Resolve<CategoryListViewModel>();
            var discountModel = _scope.Resolve<DiscountListViewModel>();
            var storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            
            productModel.StoreId = storeId;
            if (ModelState.IsValid)
            {
                productModel.Resolve(_scope);
                try
                {
                    await productModel.UpdateProduct();
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
                        Message = "There was a problem in Updating the Product.",
                        Type = ResponseTypes.Warning
                    });
                }
            }
            productModel.discountList = await discountModel.GetDiscountsAsync(storeId);
            productModel.categoryList = await categoryModel.GetCategoriesAsync(storeId);
          
            return View(productModel);
        }
        public async Task<JsonResult> GetSubCategoryByCategoryId(int categoryId)
        {
            try
            {
                var model = _scope.Resolve<SubCategoryListViewModel>();
                var data = await model.GetSubCategoriesByCategoryIdAsync(categoryId);
                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }

        //Delete Product
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = _scope.Resolve<ProductListViewModel>();
                await model.DeleteProduct(id);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfuly Deleted the Product.",
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