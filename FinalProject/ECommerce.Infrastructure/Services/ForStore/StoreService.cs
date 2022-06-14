using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using StoreEntity = ECommerce.Core.Entities.Stores.Store;



namespace ECommerce.Infrastructure.Services.ForStore
{
    public class StoreService : IStoreService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreService> _logger;


        public StoreService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<StoreService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region Asynchronus Methods
        public async Task<Store> GetStoreBySubDomainAsync(string subDomainName)
        {
            try
            {
                var storeEntity = await _ecommerceUnitOfWork.Stores.GetByIncludeAsync(x
                          => x.SubDomain == subDomainName,
                    "Email,Phone,Address,StoreImages,StoreStatus,ApplicationUser");
                var store = _mapper.Map<Store>(storeEntity);

                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(IList<Store> stores, int storeCount)> GetStoreByUserIdAsync(Guid Id)
        {

            try
            {
                var storeCount = await _ecommerceUnitOfWork.Stores.GetCountAsync(s
                        => s.ApplicationUserId == Id);
                var storeEntities = await _ecommerceUnitOfWork.Stores.GetAsync(x
                    => x.ApplicationUserId == Id, "Phone,Email,Address");

                List<Store> stores = new List<Store>();

                if (storeCount > 0)
                {
                    foreach (StoreEntity entity in storeEntities)
                    {
                        stores.Add(_mapper.Map<Store>(entity));
                    }
                    return (stores, storeCount);
                }
                else
                {
                    return (null, storeCount);
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task CreateStoreAsync(Store store)
        {
            try
            {
                var storeCount = await _ecommerceUnitOfWork.Stores
                        .GetCountAsync(s => s.StoreName == store.StoreName);

                if (storeCount == 0)
                {
                    var entity = _mapper.Map<StoreEntity>(store);

                    await _ecommerceUnitOfWork.Stores.AddAsync(entity);
                }
                else
                {
                    throw new DuplicateDataException("Store with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UpdateStoreAsync(Store store)
        {
            try
            {
                var storeCount = await _ecommerceUnitOfWork.Stores
                        .GetCountAsync(s => s.StoreName == store.StoreName && s.Id != store.Id);

                if (storeCount == 0)
                {
                    var storeEntity = await _ecommerceUnitOfWork.Stores
                        .GetByIncludeAsync(x => x.Id == store.Id, "Address,Email,Phone");
                    storeEntity = _mapper.Map(store, storeEntity);
                }
                else
                {
                    throw new DuplicateDataException("Store with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task ChangeStoreStatusAsync(Store store)
        {
            try
            {
                var storeEntity = await _ecommerceUnitOfWork.Stores.GetByIdAsync(store.Id);

                storeEntity = _mapper.Map(store, storeEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task DeleteStoreAsync(int id)
        {
            try
            {
                var storeEntity = await _ecommerceUnitOfWork.Stores.GetByIncludeAsync(x =>
                    x.Id == id, "Email,Phone,Address,StoreImages,Notifications,StoreStatus,Products,Stock");
                await _ecommerceUnitOfWork.Stores.RemoveAsync(storeEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<Store> GetStoreAsync(int id)
        {
            try
            {
                var storeEntity = _ecommerceUnitOfWork.Stores.GetAsync(x => x.Id == id,
                        "Email,Address,Phone,StoreStatus").Result.FirstOrDefault();


                var store = _mapper.Map<Store>(storeEntity);

                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<IList<Store>> GetStoresAsync()
        {
            try
            {
                var storeEntities = await _ecommerceUnitOfWork.Stores.GetAllAsync();

                List<Store> stores = new List<Store>();

                await Task.Run(() =>
                {
                    foreach (StoreEntity entity in storeEntities)
                    {
                        storeEntities.Add(_mapper.Map<StoreEntity>(entity));
                    }
                });

                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(int total, int totalDisplay, IList<Store> records)> GetStoresAsync(
            int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<Store> stores = new List<Store>();

            var result = await _ecommerceUnitOfWork.Stores.GetDynamicAsync
                (x => x.StoreName != null && x.StoreName.Contains(searchText),
                orderBy, "Email,Phone,Address", pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (StoreEntity entity in result.data)
                {
                    stores.Add(_mapper.Map<Store>(entity));
                }
            });
            return (result.total, result.totalDisplay, stores);
        }
        public async Task<(int total, int totalDisplay, IList<Store> records)> GetStoresByUserIdAsync(
           Guid UserId, int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<Store> stores = new List<Store>();

            var result = await _ecommerceUnitOfWork.Stores.GetDynamicAsync
                (x => x.StoreName != null && x.ApplicationUserId == UserId && x.StoreName.Contains(searchText),
                orderBy, "Email,Phone,Address,StoreStatus", pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (StoreEntity entity in result.data)
                {
                    stores.Add(_mapper.Map<Store>(entity));
                }
            });


            return (result.total, result.totalDisplay, stores);
        }
        #endregion

        #region Non-Asynchronus Methods
        public void CreateStore(Store store)
        {
            try
            {
                var storeCount = _ecommerceUnitOfWork.Stores
                        .GetCount(s => s.StoreName == store.StoreName);

                if (storeCount == 0)
                {
                    var entity = _mapper.Map<StoreEntity>(store);

                    _ecommerceUnitOfWork.Stores.Add(entity);
                }
                else
                {
                    throw new DuplicateDataException("Store with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void UpdateStore(Store store)
        {
            try
            {
                var storeCount = _ecommerceUnitOfWork.Stores
                        .GetCount(s => s.StoreName == store.StoreName && s.Id != store.Id);

                if (storeCount == 0)
                {
                    var storeEntity = _ecommerceUnitOfWork.Stores.GetById(store.Id);
                    //var emailEntity=_ecommerceUnitOfWork.e
                    storeEntity = _mapper.Map(store, storeEntity);
                }
                else
                {
                    throw new DuplicateDataException("Store with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void DeleteStore(int id)
        {
            try
            {
                _ecommerceUnitOfWork.Stores.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public Store GetStore(int id)
        {
            try
            {
                var storeEntity = _ecommerceUnitOfWork.Stores.GetById(id);
                var store = _mapper.Map<Store>(storeEntity);
                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public IList<Store> GetStores()
        {
            try
            {
                var storeEntities = _ecommerceUnitOfWork.Stores.GetAll();

                List<Store> stores = new List<Store>();

                foreach (StoreEntity entity in storeEntities)
                {
                    stores.Add(_mapper.Map<Store>(entity));
                }
                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public (int total, int totalDisplay, IList<Store> records) GetStores(int pageIndex,
            int pageSize, string searchText, string orderBy)
        {
            List<Store> stores = new List<Store>();

            var result = _ecommerceUnitOfWork.Stores.GetDynamic
                (x => x.StoreName != null && x.StoreName.Contains(searchText),
                orderBy, "Email,Address,StoreStatus", pageIndex, pageSize, true);

            foreach (StoreEntity entity in result.data)
            {
                stores.Add(_mapper.Map<Store>(entity));
            }
            return (result.total, result.totalDisplay, stores);
        }
        #endregion
    }
}