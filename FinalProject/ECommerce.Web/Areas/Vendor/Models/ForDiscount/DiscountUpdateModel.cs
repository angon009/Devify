using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForDiscount;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;

namespace ECommerce.Web.Areas.Vendor.Models.ForDiscount
{
    public class DiscountUpdateModel
    {
        public int Id { get; set; }
        public string? DiscountName { get; set; }
        public int? Percentage { get; set; }
        public double? Amount { get; set; }
        public string? Details { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int StoreId { get; set; }
        private IDiscountUnit _discountUnit;
        private IDiscountService _discountService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public DiscountUpdateModel()
        {

        }
        public DiscountUpdateModel(IDiscountUnit discountUnit, IMapper mapper
            , ILifetimeScope scope,IDiscountService discountService)
        {
            _mapper = mapper;
            _scope = scope;
            _discountUnit = discountUnit;
            _discountService = discountService;

        }
        public void Resolve(ILifetimeScope scope)
        {
            _discountUnit = scope.Resolve<IDiscountUnit>();
            _mapper = scope.Resolve<IMapper>();
            _scope = scope;
        }
        public async Task<DiscountUpdateModel> LoadDataAsync(int Id)
        {
            try
            {
                var modelData= await _discountService.GetDiscountAsync(Id);
                return _mapper.Map(modelData, this);
              
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateDiscountAsync()
        {
            try
            {
                var discount = _mapper.Map<Discount>(this);
                await _discountUnit.UpdateServiceAsync(discount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
