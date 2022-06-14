using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Core.StoredProcedureEntites.SqlEnitities;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public class InventoryAlertRepository : Repository<InventoryAlert, int>, IInventoryAlertRepository
    {
        public InventoryAlertRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
        public async Task<(IEnumerable<StockProduct> data, int total, int totalFiltered)> GetInventoryProductsAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId,
            int MinimumQuantity)
        {

            var result = await QueryWithStoredProcedureAsync<StockProduct>("GetInventory", new Dictionary<string, object>
                {
                    {"PageIndex", pageIndex},
                    {"PageSize", pageSize},
                    {"OrderBy", orderBy},
                    {"StoreId", storeId},
                    {"MinimumQuantity", MinimumQuantity}

                },
                new Dictionary<string, Type>
                {
                    {"Total", typeof(int)},
                    {"TotalDisplay", typeof(int)}
                });

            return (result.result, int.Parse(result.outValues.ElementAt(0).Value.ToString()!),
                int.Parse(result.outValues.ElementAt(1).Value.ToString()!));
        }

        public async Task<IEnumerable<DashboardValues>> GetDashboardValues(int StoreId)
        {
            var result = await QueryWithSqlAsync<DashboardValues>($@"select count(*) as DashboardValue from products  where products.StoreId={StoreId} 
                        union all
                        select count(*)  from Orders where OrderStatusId != 4  and StoreId = {StoreId}
                        union all
                        select count(*)  from Orders where OrderStatusId = 3 and StoreId = {StoreId}");
            return result.result;
        }
    }
}
