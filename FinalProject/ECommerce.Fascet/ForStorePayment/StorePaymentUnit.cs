using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.Services.ForStorePayments;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Fascet.ForStorePayment
{
    public class StorePaymentUnit : IStorePaymentUnit
    {
        private IStorePaymentService _storePaymentService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public StorePaymentUnit(IStorePaymentService storePaymentService, IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _storePaymentService = storePaymentService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        public async Task CreateServiceAsync(StorePayments storePayments)
        {
            await _storePaymentService.CreatePaymentAsync(storePayments);
            await _ecommerceUnitOfWork.SaveAsync();
        }

        public Task DeleteServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateServiceAsync(StorePayments item)
        {
            throw new NotImplementedException();
        }


        public void CreateService(StorePayments item)
        {
            throw new NotImplementedException();
        }
        public void DeleteService(int id)
        {
            throw new NotImplementedException();
        }
        public void UpdateService(StorePayments item)
        {
            throw new NotImplementedException();
        }
    }
}
