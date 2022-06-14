using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Orders;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForCart
{
    public class CartRepository : Repository<Cart, Guid>, ICartRepository
    {
        public CartRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }
    }
}
