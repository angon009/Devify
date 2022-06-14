using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }


        public async Task<(IEnumerable<FilteredProducts> data, int total, int totalFiltered)> GetProductsAsync(
            int pageIndex,
            int pageSize,
            string? orderBy,
            int storeId,
            int? categoryId,
            int? subcategoryId,
            string? brands,
            int? minimum,
            int? maximum)
        {

            var result = await QueryWithStoredProcedureAsync<FilteredProducts>("GetProducts", new Dictionary<string, object>
                {
                    {"PageIndex", pageIndex},
                    {"PageSize", pageSize},
                    {"OrderBy", orderBy},
                    {"StoreId", storeId},
                    {"CategoryId", categoryId},
                    {"SubcategoryId", subcategoryId},
                    {"Brand", brands},
                    {"Minimum", minimum},
                    {"Maximum", maximum}
                },
                new Dictionary<string, Type>
                {
                    {"Total", typeof(int)},
                    {"TotalDisplay", typeof(int)}
                });

            return (result.result, int.Parse(result.outValues.ElementAt(0).Value.ToString()!),
                int.Parse(result.outValues.ElementAt(1).Value.ToString()!));
        }
    }
}
