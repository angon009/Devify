using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForStorePayment;
using ECommerce.Infrastructure.BusinessObjects.Common;

namespace ECommerce.Web.Areas.Vendor.Models.BillPayModels
{
    public class CreateStorePaymentModel
    {
        private IStorePaymentUnit _storePaymentUnit;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public CreateStorePaymentModel(IStorePaymentUnit storePaymentUnit, IMapper mapper)
        {
            _storePaymentUnit = storePaymentUnit;
            _mapper = mapper;
        }
        public CreateStorePaymentModel()
        {

        }
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int StoreId { get; set; }
        public double PaymentAmount { get; set; }
        public string TransactionId { get; set; }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _storePaymentUnit = _scope.Resolve<IStorePaymentUnit>();
            _mapper = _scope.Resolve<IMapper>();
        }

        internal async Task CreatePaymentAsync()
        {
            var storePayment = _mapper.Map<StorePayments>(this);
            await _storePaymentUnit.CreateServiceAsync(storePayment);
        }
    }
}
