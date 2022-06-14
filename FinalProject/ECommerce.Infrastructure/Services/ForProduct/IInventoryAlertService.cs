using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Core.StoredProcedureEntites.SqlEnitities;
using ECommerce.Infrastructure.BusinessObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public interface IInventoryAlertService
    {
        Task CreateOrUpdateInventoryAlertAsync(InventoryAlert inventory);
        Task<InventoryAlert> GetInventoryAlertByStoreIdAsync(int storeId);
        Task<(int total, int totalDisplay, IList<StockProduct> records)> GetInventoryAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId,
            int MinimumQuantity);
        Task<(int total, int totalDisplay, IList<StockProduct> records)> GetInventoryAlertAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId);
        Task<(int TotalProducts, int TotalOrders, int TotalSales)> GetDashboardValues(int StoreId);
    }

}
