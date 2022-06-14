using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForCategory
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
