using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForOrder;

namespace ECommerce.Web.Models
{
    public class OrderHistoryModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public List<Order> Orders { get; set; }
        public string ImageUrl { get { return GetImage.Url; } }
        public OrderHistoryModel(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task GetOrders(string userId)
        {
            Orders = await _orderService.GetOrdersAsync(userId);
        }
        public async Task CancelOrder(int id, string userId)
        {
            var cancelId = 4;
            await _orderService.ChangeOrderStatusAsync(id, userId, cancelId);
        }
    }
    
}
