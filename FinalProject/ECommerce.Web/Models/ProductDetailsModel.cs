using ECommerce.Infrastructure.Services.ForProduct;

namespace ECommerce.Web.Models
{
    public class ProductDetailsModel
    {
        private readonly IProductService _productService;

        public string ImageUrl { get { return GetImage.Url; } }
        public ProductDetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        internal async Task<object> GetProductAsync(int id)
        {
            return await _productService.GetStoreProductAsync(id);
        }

        internal async Task<object> GetColorsAsync(int productId)
        {
            return await _productService.GetProductColorsAsync(productId);
        }
    }
    public static class GetImage
    {
        public static string Url => "/Images/Stores/";
    }
}
