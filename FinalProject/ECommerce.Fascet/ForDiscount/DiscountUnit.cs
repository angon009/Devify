using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Fascet.ForDiscount
{
    public class DiscountUnit : IDiscountUnit
    {
        private IDiscountService _discountService;
        private IEcommerceUnitOfWork _ecommerceUnitOfWork;
        public DiscountUnit(IDiscountService discountService, IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _discountService = discountService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        public void CreateService(Discount item)
        {
            throw new NotImplementedException();
        }

        public async Task CreateServiceAsync(Discount item)
        {
            await _discountService.CreateDiscountAsync(item);
            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async void DeleteService(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _discountService.DeleteDiscountAsync(id);
            await  _ecommerceUnitOfWork.SaveAsync();
        }

        public void UpdateService(Discount item)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateServiceAsync(Discount item)
        {
           await _discountService.UpdateDiscountAsync(item);
           await _ecommerceUnitOfWork?.SaveAsync()!;
        }
    }
}
