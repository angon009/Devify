using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Orders;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForOrder
{
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }

        public async Task<IList<OrderCount>> GetSumAsync(int storeId, DateTime from, DateTime to)
        {
            var result = await QueryWithSqlAsync<OrderCount>(
                @$"select Count(*) as CountTotal, Sum(Orders.TotalAmount) as SumTotal, Sum(Orders.TotalCostAmount) as TotalCost, Sum(Orders.DiscountTotal) as SumDiscount
                    from Orders where Orders.OrderStatusId = 3 and Orders.StoreId = {storeId} and OrderDate between '{from}' and '{to}'; "
                 , null, null);
            return result.result;
        }
        public async Task<IList<CustomerList>> GetCustomerAsync(int storeId)
        {
            var result = await QueryWithSqlAsync<CustomerList>(
                @$"Select AspNetUsers.FirstName,AspNetUsers.Email, AspNetUsers.PhoneNumber, 
                Sum(TotalAmount) as TotalAmount, Sum(Orders.DiscountTotal) as TotalDiscount,
                COUNT(ApplicationUserId) as TotalOrders, 
                CONCAT( Addresses.District,', ', Addresses.Thana,' ',Addresses.RoadNumber ) as Address
                from Orders inner join AspNetUsers on AspNetUsers.Id = Orders.ApplicationUserId
                left join Addresses on Addresses.ApplicationId = AspNetUsers.Id
                where Orders.StoreId = {storeId} and Orders.OrderStatusId = 3
                Group by Orders.ApplicationUserId, AspNetUsers.FirstName, Addresses.District,
                Addresses.Thana, Addresses.RoadNumber, AspNetUsers.Email, AspNetUsers.PhoneNumber;"
                    , null, null);
            return result.result;
        }
    }
}
