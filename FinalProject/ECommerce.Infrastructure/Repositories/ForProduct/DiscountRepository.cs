using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Products;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public class DiscountRepository : Repository<Discount, int>, IDiscountRepository
    {
        public DiscountRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }
    }
}
