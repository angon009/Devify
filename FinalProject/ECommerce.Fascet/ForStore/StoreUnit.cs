using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForStore
{
    public class StoreUnit : IStoreUnit
    {
        private IStoreService _storeService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public StoreUnit(IEcommerceUnitOfWork ecommerceUnitOfWork, IStoreService storeService)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _storeService = storeService;
        }
        #region Asynchronous Methods
        public async Task CreateServiceAsync(Store store)
        {
            await _storeService.CreateStoreAsync(store);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async Task UpdateServiceAsync(Store store)
        {
            await _storeService.UpdateStoreAsync(store);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        public async Task StoreEnableServiceAsync(int id)
        {
            var store = await _storeService.GetStoreAsync(id);
            store.StoreStatusId = 1;

            await _storeService.ChangeStoreStatusAsync(store);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        public async Task StoreDisableServiceAsync(int id)
        {
            var store = await _storeService.GetStoreAsync(id);
            store.StoreStatusId = 2;

            await _storeService.ChangeStoreStatusAsync(store);

            await _ecommerceUnitOfWork.SaveAsync();
        } 
        public async Task StoreOnDeleteServiceAsync(int id)
        {
            var store = await _storeService.GetStoreAsync(id);
            store.StoreStatusId = 3;

            await _storeService.ChangeStoreStatusAsync(store);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        public async Task DeleteServiceAsync(int id)
        {
            await _storeService.DeleteStoreAsync(id);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        public async Task<(int total, int totalDisplay, IList<Store> records)> GetServiceAsync(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            var data = await _storeService.GetStoresAsync(pageIndex, pageSize, searchText, orderBy);
            return (data.total, data.totalDisplay, data.records);
        }
        //get methods needs to move to service Layer
        public async Task<(IList<Store> stores, int storeCount)> GetServiceAsync(Guid Id)
        {
            var data = await _storeService.GetStoreByUserIdAsync(Id);
            return (data.stores, data.storeCount);
        }

         
        #endregion

        #region Non-Asynchronous Methods
        public void CreateService(Store store)
        {
            _storeService.CreateStore(store);

            _ecommerceUnitOfWork.Save();
        }

        public void UpdateService(Store store)
        {
            _storeService.UpdateStoreAsync(store);

            _ecommerceUnitOfWork.Save();
        }

        public void DeleteService(int id)
        {
            _storeService.DeleteStore(id);

            _ecommerceUnitOfWork.Save();
        }
        //get methods needs to move to service Layer
        public (int total, int totalDisplay, IList<Store> records) GetService(
            int pageIndex, int pageSize, string searchText, string orderBy)
        {
            var data = _storeService.GetStores(pageIndex, pageSize, searchText, orderBy);
            return (data.total, data.totalDisplay, data.records);
        }
        //get methods needs to move to service Layer
         

        #endregion


    }
}
