using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Utility;

namespace ECommerce.Web.Models
{
    public class StoreModel
    {
        private readonly IProductService _productService;
        private IStoreService _storeService;

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public StoreModel(IProductService productService, IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }
        public string BannerUrl { get; set; }
        public string ProductImageUrl { get { return "/Images/Stores/"; } }

        internal async Task<IList<ECommerce.Infrastructure.BusinessObjects.Products.Product>>
            GetProducts(int storeId) //Store id Must provided as parameter
        {
            return await _productService.GetStoreProductsAsync(storeId);
        }

        internal async Task<object> GetCategories(int storeId) => await _productService.GetCategoryAsync(storeId);
        public async Task<int> GetStoreIdBySubDomain()
        {
            string subDomain = UrlAction.GetSubDomain();
            Store store = await _storeService.GetStoreBySubDomainAsync(subDomain);
            if (store != null)
            {
                return store.Id;
            }
            return 0;
        }
    }
}