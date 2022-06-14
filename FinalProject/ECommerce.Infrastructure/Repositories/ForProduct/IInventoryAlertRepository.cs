using ECommerce.Core.Entities.Products;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Core.StoredProcedureEntites.SqlEnitities;
using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public interface IInventoryAlertRepository:IRepository<InventoryAlert, int>
    {
        Task<(IEnumerable<StockProduct> data, int total, int totalFiltered)> GetInventoryProductsAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId,
            int MinimumQuantity);
        Task<IEnumerable<DashboardValues>> GetDashboardValues(int StoreId);
    }
}
