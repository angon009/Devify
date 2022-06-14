using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForStock;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForStock
{
    public class StockUnit : IStockUnit
    {
        private IStockService _stockService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public StockUnit(IStockService stockService,
            IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _stockService = stockService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        #region Asynchronous Methods 

        //public async Task UpdateServiceAsync(Stock stock)
        //{
        //    await _stockService.UpdateStockAsync(stock);

        //    await _ecommerceUnitOfWork.SaveAsync();
        //}

        public async Task DeleteServiceAsync(int id)
        {

        }

        public Task CreateServiceAsync(Stock item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateServiceAsync(Stock item)
        {
            throw new NotImplementedException();
        }

        public void CreateService(Stock item)
        {
            throw new NotImplementedException();
        }

        public void UpdateService(Stock item)
        {
            throw new NotImplementedException();
        }

        public void DeleteService(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateQuantityServiceAsync(int id, int quantity)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
