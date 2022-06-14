using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Infrastructure.Services.ForStock
{
    public interface IStockService
    {
        #region Asynchronus Methods 
        Task<Product> GetStockAsync(int id); // For Read
        Task<Product> GetStoreStockAsync(int id); //With including properties
        Task<IList<Product>> GetStoreStocksAsync(int storeId); // Get Products with storeId
        Task<IList<Product>> GetStocksAsync(); // For Read 
        Task<(int total, int totalDisplay, IList<Product> records)> GetStocksAsync(int pageIndex,
            int pageSize, string searchText, string orderBy); //For Read  
        Task<(int total, int totalDisplay, IList<Product> records)> GetStocksAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy);
        Task<(int total, int totalDisplay, IList<FilteredProducts> records)> GetStocksAsync(
            int pageIndex, int pageSize, int storeId, int? categoryId, int? subCategoryId, string brands, int? minimum,
            int? maximum, string orderBy); //get products using stored procedure
          

        #endregion
    }
}
