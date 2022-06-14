using ECommerce.Core.Entities;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ForOrder
{
    public interface IOrderService
    {
        Task OrderAsync(List<Order> orders);
        Task<List<Order>> GetOrdersAsync(string userId);
        Task<Order> GetOrderAsync(int orderId);
        Task ChangeOrderStatusAsync(int id, int statusId);
        Task ChangeOrderStatusAsync(int id, string userId, int statusId);
        Task<(int total, int totalDisplay, IList<Order> records)> GetOrdersAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy);
        Task<int> GetSalesReport(int storeId);
        Task<(int total, int totalDisplay, IList<Order> records)> GetStoreSalesAsync(
            int storeId, int pageIndex, int pageSize, DateTime from, DateTime to, string orderBy);
        Task<(double totalAmount, double totalCostAmount, double totalDiscount, int totalSale)> GetPriceSumAsync(int storeId, DateTime from, DateTime to);
        Task<IList<CustomerList>> GetCustomersAsync(int storeId);
    }
}
