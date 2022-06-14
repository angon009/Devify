using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForSubCategory
{
    public class SubCategoryRepository : Repository<SubCategory, int>, ISubCategoryRepository
    {
        public SubCategoryRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
