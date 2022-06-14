using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Stores;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForStock
{
    public class StockRepository : Repository<Stock, int>, IStockRepository
    {
        public StockRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }

    }
}
