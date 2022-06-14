using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForDiscount;
using ECommerce.Infrastructure.BusinessObjects.Products;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.Vendor.Models.ForDiscount
{
    public class DiscountCreateModel
    {
        [Display(Name="Discount Name")]
        [Required]
        public string? DiscountName { get; set; }
        [Required]
        [Display(Name = "Discount Amount In Percentage")]
        public int? Percentage { get; set; }
        public double? Amount { get; set; }
        public string? Details { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Required]
        [Display(Name = "Expire Date")]
        public DateTime? ExpireDate { get; set; }
        public int StoreId { get; set; }
        private IDiscountUnit _discountUnit;
        private IMapper _mapper;
        private ILifetimeScope _scope;        
        public DiscountCreateModel()
        {

        }
        public DiscountCreateModel(IDiscountUnit discountUnit, IMapper mapper
            , ILifetimeScope scope)
        {
            _mapper = mapper;
            _scope = scope;
            _discountUnit=discountUnit;

        }
        public void Resolve(ILifetimeScope scope)
        {
            _discountUnit = scope.Resolve<IDiscountUnit>();
            _mapper = scope.Resolve<IMapper>();
            _scope = scope;
        }
        public async Task CreateDiscountAsync()
        {
            try
            {
                var discount = _mapper.Map<Discount>(this);
                await _discountUnit.CreateServiceAsync(discount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
