using ECommerce.Core.Entities.Products;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Data;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<(IEnumerable<FilteredProducts> data, int total, int totalFiltered)> GetProductsAsync(
            int pageIndex,
            int pageSize,
            string? orderBy,
            int storeId,
            int? categoryId,
            int? subcategoryId,
            string? brands,
            int? minimum,
            int? maximum);
    }
}

