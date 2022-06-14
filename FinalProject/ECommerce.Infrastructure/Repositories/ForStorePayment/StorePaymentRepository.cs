using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Common;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Repositories.ForStorePayment
{
    public class StorePaymentRepository : Repository<StorePayments, int>, IStorePaymentRepository
    {
        public StorePaymentRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
