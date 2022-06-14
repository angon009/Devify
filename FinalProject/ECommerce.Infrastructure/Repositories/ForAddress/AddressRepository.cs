using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Stores;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForAddress
{
    public class AddressRepository : Repository<Address, int>, IAddressRepository
    {
        public AddressRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }
    }
}
