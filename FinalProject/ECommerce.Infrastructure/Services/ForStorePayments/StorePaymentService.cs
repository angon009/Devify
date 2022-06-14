using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using StorePaymentEntity = ECommerce.Core.Entities.Common.StorePayments;

namespace ECommerce.Infrastructure.Services.ForStorePayments
{
    public class StorePaymentService : IStorePaymentService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StorePaymentService> _logger;

        public StorePaymentService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<StorePaymentService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreatePaymentAsync(StorePayments storePayments)
        {
            try
            {
                var entity = _mapper.Map<StorePaymentEntity>(storePayments);
                await _ecommerceUnitOfWork.StorePayments.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<StorePayments> GetStorePaymentAsync(int id)
        {
            try
            {
                var storePaymentEntity = await _ecommerceUnitOfWork.StorePayments.GetByIdAsync(id);
                StorePayments storePayments = new StorePayments();
                await Task.Run(() =>
                {
                    storePayments = _mapper.Map<StorePayments>(storePayments);
                });
                return storePayments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public  async Task<(int total, int totalDisplay, IList<StorePayments> records)> 
            GetStorePaymentsAsync(int storeId, int pageIndex, int pageSize, 
            string searchText, string orderBy)
        {
            var result = await _ecommerceUnitOfWork.StorePayments.GetDynamicAsync(
                x => x.StoreId == storeId && x.TransactionId.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);
            IList<StorePayments> storePayments = new List<StorePayments>();

            await Task.Run(() =>
            {
                foreach(var entity in result.data)
                {
                    storePayments.Add(_mapper.Map<StorePayments>(entity));
                }
            });

            return (result.total, result.totalDisplay, storePayments);
        }

        public async Task<(int total, int totalDisplay, IList<StorePayments> records)> 
            GetStorePaymentsAsync(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            var result = await _ecommerceUnitOfWork.StorePayments.GetDynamicAsync(
                x => x.Store.StoreName.Contains(searchText) || 
                x.TransactionId.Contains(searchText),
                orderBy, "Store", pageIndex, pageSize, true);
            IList<StorePayments> storePayments = new List<StorePayments>();

            await Task.Run(() =>
            {
                foreach (var entity in result.data)
                {
                    storePayments.Add(_mapper.Map<StorePayments>(entity));
                }
            });

            return (result.total, result.totalDisplay, storePayments);
        }
    }
}