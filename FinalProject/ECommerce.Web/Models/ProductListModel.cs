using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.BusinessObjects.Products;
using System.Text;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ECommerce.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Models
{
    public class ProductListModel
    {
        private readonly IProductService _productService;
        public int StoreId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? MinimumPrice { get; set; }
        public int? MaximumPrice { get; set; }
        public string Brand { get; set; }
        public int PageIndex { get; set; }


        public ProductListModel()
        {
            
        }
        public ProductListModel(IProductService productService)
        {
            _productService = productService;
        }


        public string ProductImageUrl { get { return "/ProductImages/"; } }
        internal async Task<object> GetCategories(int storeId) => await _productService.GetCategoryAsync(storeId);
        internal async Task<object> GetBrands(int storeId) => await _productService.GetBrandsAsync(storeId);
        internal async Task<(int total, int filtered, IList<ECommerce.Core.StoredProcedureEntites.FilteredProducts>)> GetFilteredProducts()
        {
            var data = _productService.GetProductsAsync(
                PageIndex,
                10,
                StoreId,
                CategoryId,
                SubCategoryId,
                Brand!,
                MinimumPrice,
                MaximumPrice,
                "Brand");
            var model = new List<ECommerce.Core.StoredProcedureEntites.FilteredProducts>();
            foreach (var product in data.Result.records)
            {
                model.Add(product);
            }
            return (data.Result.total, data.Result.totalDisplay, model);
        }

    }
}
