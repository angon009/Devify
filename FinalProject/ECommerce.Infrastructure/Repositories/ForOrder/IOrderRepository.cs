using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Orders;
using ECommerce.Data;

namespace ECommerce.Infrastructure.Repositories.ForOrder
{
    public interface IOrderRepository: IRepository<Order, int>
    {
        Task<IList<OrderCount>> GetSumAsync(int storeId, DateTime from, DateTime to);
        Task<IList<CustomerList>> GetCustomerAsync(int storeId);
    }
}
