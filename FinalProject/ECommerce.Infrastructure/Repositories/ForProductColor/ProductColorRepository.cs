using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForProductColor
{
    public class ProductColorRepository : Repository<ProductColor, int>, IProductColorRepository
    {
        public ProductColorRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
