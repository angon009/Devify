using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Stores;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForStore
{
    public class StoreRepository : Repository<Store, int>, IStoreRepository
    {
        public StoreRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
