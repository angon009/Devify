using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForDiscount;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Web.Models;
using ECommerce.Utility;
using ECommerce.Membership.Repositories;

namespace ECommerce.Web.Areas.Vendor.Models.ForDiscount
{
    public class DiscountListViewModel
    {
        public int Id { get; set; }
        public string? DiscountName { get; set; }
        private IDiscountService _discountService;
        private IDiscountUnit _discountUnit;
        private readonly IAccountRepository _accountRepository;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public DiscountListViewModel()
        {

        }
        public DiscountListViewModel(IDiscountService discountService, IMapper mapper
            , ILifetimeScope scope, IDiscountUnit discountUnit, 
            IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _scope = scope;
            _discountService = discountService;
            _discountUnit = discountUnit;
            _accountRepository = accountRepository;
        }
        public async Task<List<DiscountListViewModel>> GetDiscountsAsync(int StoreId)
        {
            var discountList = await _discountService.GetDiscountsAsync(StoreId);
            List<DiscountListViewModel> discountViewList = new();
            foreach (var discount in discountList)
            {
                discountViewList.Add(_mapper.Map<DiscountListViewModel>(discount));
            }
            return discountViewList;
        }
        public async Task DeleteDiscount(int Id)
        {
            await _discountUnit.DeleteServiceAsync(Id);
        }
        public async Task<object> GetPagedDiscounts(DataTablesAjaxRequestModel model, int StoreId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _discountService.GetDiscountsAsync(
                StoreId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "DiscountName", "Percentage", "Details", "StartDate", "ExpireDate"}));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.DiscountName,
                                record.Percentage.ToString(),
                                record.Details,
                                record.StartDate.CurrentZone(user.TimeZone!).ToString(),
                                record.ExpireDate.CurrentZone(user.TimeZone!).ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
