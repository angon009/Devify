using Autofac;
using AutoMapper;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Services.ForOrder;

namespace ECommerce.Web.Areas.Vendor.Models.CustomerModels
{
    public class CustomerListViewModel
    { 
        private IMapper _mapper;
        private IOrderService _orderService;
        public CustomerListViewModel()
        {

        }
        public CustomerListViewModel(IOrderService orderService, IMapper mapper)
        { 
            _orderService = orderService;
            _mapper = mapper;
        }
         
        public async Task<object> GetCustomersAsync(int storeId)
        {
            var customers = await _orderService.GetCustomersAsync(storeId); 
            return new
            {
                recordsTotal = customers.Count, 
                data = (from record in customers
                        select new string[]
                        {
                                record.FirstName!, 
                                record.Address!, 
                                record.Email!,
                                record.PhoneNumber!,
                                record.TotalOrders.ToString()!,
                                record.TotalAmount.ToString()!,
                                record.TotalDiscount.ToString()!  
                        }
                    ).ToArray()
            };
        }
    }
}
