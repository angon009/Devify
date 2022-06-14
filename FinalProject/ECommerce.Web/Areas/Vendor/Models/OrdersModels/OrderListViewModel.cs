using Autofac;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Infrastructure.Services.ForOrder;
using ECommerce.Web.Models;
using ECommerce.Utility;
using ECommerce.Membership.Repositories;

namespace ECommerce.Web.Areas.Vendor.Models.OrdersModels
{
    public class OrderListViewModel
    {
        private IOrderService _orderService;
        private IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private ILifetimeScope _scope;
        public string ImageUrl { get { return GetImage.Url; } }
        public Order OrderDetail { get; set; }
        public string From { get; set; }

        #region ForPostMethod
        public int OrderStatusId { get; set; }
        public int OrderId { get; set; }
        #endregion
        public OrderListViewModel()
        {

        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _orderService = _scope.Resolve<IOrderService>();
            _mapper = _scope.Resolve<IMapper>();
        }
        public OrderListViewModel(IOrderService orderService,
            IMapper mapper, IAccountRepository accountRepository)
        {
            _orderService = orderService;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
        public async Task<object> GetOrdersAsync(DataTablesAjaxRequestModel model, int storeId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _orderService.GetOrdersAsync(
                storeId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "OrderDate" }));

            var result = new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.ApplicationUser.FirstName+" "+record.ApplicationUser.LastName,
                            record.ApplicationUser.Email,
                            record.TotalAmount.ToString(),
                            record.DiscountTotal.ToString(),
                            (record.OrderStatusId==1)?"Pending":(record.OrderStatusId==2)?
                            "Ondelivery":(record.OrderStatusId==3)?"Delivered":"Cancelled",
                            record.OrderDate.CurrentZone(user.TimeZone!).ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
            return result;
        }
        public async Task GetOrderAsync(int orderId)
        {
            OrderDetail = await _orderService.GetOrderAsync(orderId);
        }
        public async Task ChangeStatusAsync()
        {
            await _orderService.ChangeOrderStatusAsync(OrderId, OrderStatusId);
        }
    }
}
