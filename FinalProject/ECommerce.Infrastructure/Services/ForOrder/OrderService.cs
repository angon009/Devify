using AutoMapper;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using OrderEntity = ECommerce.Core.Entities.Orders.Order;

namespace ECommerce.Infrastructure.Services.ForOrder
{
    public class OrderService : IOrderService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IEcommerceUnitOfWork ecommerceUnitOfWork,
            IMapper mapper, ILogger<OrderService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task OrderAsync(List<Order> orders)
        {
            try
            {
                foreach (var order in orders)
                {
                    var orderEntity = _mapper.Map<OrderEntity>(order);
                    await _ecommerceUnitOfWork.Orders.AddAsync(orderEntity);
                }
                await _ecommerceUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<List<Order>> GetOrdersAsync(string userId)
        {
            try
            {
                var orderEntities = await _ecommerceUnitOfWork.Orders.GetAsync(x =>
                                x.ApplicationUserId == Guid.Parse(userId),
                                "OrderDetails,OrderDetails.Product,OrderDetails.Product.ProductImages");
            var orders = _mapper.Map<List<Order>>(orderEntities);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(int total, int totalDisplay, IList<Order> records)> GetOrdersAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<Order> orders = new List<Order>();

            var result = await _ecommerceUnitOfWork.Orders.GetDynamicAsync
                (x => x.StoreId == storeId,
                orderBy, "OrderDetails,OrderDetails.Product,ApplicationUser", 
                pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (var entity in result.data)
                {
                    orders.Add(_mapper.Map<Order>(entity));
                }
            });
            return (result.total, result.totalDisplay, orders);
        }
        public async Task<Order> GetOrderAsync(int orderId)
        {
            try
            {
                var orderEntity = _ecommerceUnitOfWork.Orders.GetAsync(x => x.Id == orderId,
                        "OrderDetails,OrderDetails.Product,OrderDetails.Product.ProductImages,ApplicationUser,ApplicationUser.Address")
                        .Result.FirstOrDefault();
                var order = _mapper.Map<Order>(orderEntity);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task ChangeOrderStatusAsync(int id, int statusId)
        {
            try
            {
                var order = _ecommerceUnitOfWork.Orders.GetAsync(x => x.Id == id &&
                                 x.OrderStatusId != statusId, "OrderDetails,OrderDetails.Product")
                                                .Result.FirstOrDefault();

                if (order != null)
                {
                    order.OrderStatusId = statusId;

                    if (statusId == 1)
                    {
                        foreach (var product in order!.OrderDetails!)
                        {
                            product.Product!.Quantity -= product.Quantity;
                        }
                    }
                    else if (statusId == 4)
                    {
                        foreach (var product in order!.OrderDetails!)
                        {
                            product.Product!.Quantity += product.Quantity;
                        }
                    }
                    await _ecommerceUnitOfWork.SaveAsync();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task ChangeOrderStatusAsync(int id, string userId, int statusId)
        {
            try
            {
                var order = _ecommerceUnitOfWork.Orders.GetAsync(x => x.Id == id &&
                        x.OrderStatusId == 1 && x.ApplicationUserId == Guid.Parse(userId), "OrderDetails,OrderDetails.Product")
                                                .Result.FirstOrDefault();

                if (order != null)
                {
                    order.OrderStatusId = statusId;

                    // Increase Product Quantity after cancel
                    foreach (var product in order!.OrderDetails!)
                    {
                        product.Product!.Quantity += product.Quantity;
                    }

                    await _ecommerceUnitOfWork.SaveAsync();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<int> GetSalesReport(int storeId)
        {
            var orderCount = await _ecommerceUnitOfWork.Orders.GetCountAsync(x => x.StoreId == storeId &&
            x.OrderStatusId == 3);
            return orderCount;
        }

        public async Task<(int total, int totalDisplay, IList<Order> records)> GetStoreSalesAsync(
            int storeId, int pageIndex, int pageSize, DateTime from, DateTime to, string orderBy)
        {
            var result = await _ecommerceUnitOfWork.Orders.GetDynamicAsync(x => x.StoreId == storeId
            && x.OrderStatusId == 3 && x.OrderDate >= from && x.OrderDate <= to, orderBy, string.Empty, pageIndex, pageSize, true);
            
            IList<Order> orders = new List<Order>();
            await Task.Run(() =>
            {
                foreach(var entity in result.data)
                {
                    orders.Add(_mapper.Map<Order>(entity));
                }
            });

            return (result.total, result.totalDisplay, orders);
        }

        public async Task<(double totalAmount, double totalCostAmount, double totalDiscount, int totalSale)> GetPriceSumAsync(int storeId, DateTime from, DateTime to)
        {
            var entity = await _ecommerceUnitOfWork.Orders.GetSumAsync(storeId, from, to);
            return (entity[0].SumTotal, entity[0].TotalCost, entity[0].SumDiscount, entity[0].CountTotal);
        }
        public async Task<IList<CustomerList>> GetCustomersAsync(int storeId)
        {
            var customers =  await _ecommerceUnitOfWork.Orders.GetCustomerAsync(storeId);
            return customers;
        }
    }
}