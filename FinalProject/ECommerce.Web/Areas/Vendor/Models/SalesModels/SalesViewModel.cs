using ECommerce.Infrastructure.Services.ForOrder;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Models.SalesModels
{
    public class SalesViewModel
    {
        public string BaseUrl { get; set; } = "/Images/Stores/";
        public double TotalAmount { get; set; }
        public double TotalCostAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int TotalSale { get; set; }
        public double Revenue { get { return TotalAmount - TotalDiscount; } }

        public DateTime FromDate { get; set; } = new DateTime(2020, 01, 01);
        public DateTime ToDate { get; set; } = DateTime.Now.Date.AddDays(1);

        private IOrderService _orderService;
        private readonly IAccountRepository _accountRepository;

        public SalesViewModel(IOrderService orderService, IAccountRepository accountRepository)
        {
            _orderService = orderService;
            _accountRepository = accountRepository;
        }
        public SalesViewModel()
        {

        }
        internal async Task GetOrdersAsync(int storeId)
        {
            var orderCount = await _orderService.GetSalesReport(storeId);
            TotalSale = orderCount;
        }

        internal async Task GetCountAsync(int storeId)
        {
            var result = await _orderService.GetPriceSumAsync(storeId, FromDate, ToDate);
            TotalAmount = result.totalAmount;
            TotalCostAmount = result.totalCostAmount;
            TotalDiscount = result.totalDiscount;
            TotalSale = result.totalSale;
        }

        internal async Task<object> GetAllSalesAsync(DataTablesAjaxRequestModel model, int storeId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _orderService.GetStoreSalesAsync(
                storeId,
                model.PageIndex,
                model.PageSize,
                FromDate,
                ToDate,
                model.GetSortText(new string[] { "OrderDate", "TotalAmount", "DiscountTotal" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.OrderDate.CurrentZone(user.TimeZone!).ToString("dd MMM ,yyyy"),
                            record.TotalAmount.ToString(),
                            record.DiscountTotal.ToString(),
                            record.Id.ToString()
                        }).ToArray()
            };
        }
    }
}
