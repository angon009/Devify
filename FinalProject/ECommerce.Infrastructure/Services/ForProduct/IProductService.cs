using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public interface IProductService
    {
        #region Asynchronus Methods
        Task<List<Product>> GetProductsByStoreIdAsync(int StoreId);
        Task DiscountAssignToMultiProducts(int[] productsIdlist, int discountId);
        Task CreateProductAsync(Product product); // For Create
        Task UpdateProductAsync(Product product); // For Update
        Task DeleteProductAsync(int id); // For Delete
        Task<Product> GetProductAsync(int id); // For Read
        Task<Product> GetStoreProductAsync(int id); //With including properties
        Task<IList<Product>> GetStoreProductsAsync(int storeId); // Get Products with storeId
        Task<IList<Product>> GetProductsAsync(); // For Read
        Task<IList<ProductColor>> GetProductColorsAsync(int productId);
        Task<(int total, int totalDisplay, IList<Product> records)> GetProductsAsync(int pageIndex,
            int pageSize, string searchText, string orderBy); //For Read
        Task<IList<Category>> GetCategoryAsync(int storeId); //Get Store Categories
        Task<IList<string?>> GetBrandsAsync(int storeId);
        Task<(int total, int totalDisplay, IList<Product> records)> GetProductsAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy);
        Task<(int total, int totalDisplay, IList<FilteredProducts> records)> GetProductsAsync(
            int pageIndex, int pageSize, int storeId, int? categoryId, int? subCategoryId, string brands, int? minimum,
            int? maximum, string orderBy); //get products using stored procedure
        #endregion



        #region Non-Asynchronus Methods
        void CreateProduct(Product product); // For Create
        void UpdateProduct(Product product); // For Update
        void DeleteProduct(int id); // For Delete
        Product GetProduct(int id); // For Read
        IList<Product> GetProducts(); // For Read
        (int total, int totalDisplay, IList<Product> records) GetProducts(int pageIndex, int pageSize,
            string searchText, string orderBy); //For Read 
        Task ChangeProductQuantityAsync(Product product);
        #endregion
    }
}
