using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Infrastructure.Services.ForStore
{
    public interface IStoreService
    {

        #region Asynchronus Methods
        Task<Store> GetStoreBySubDomainAsync(string subDomainName);
        Task CreateStoreAsync(Store store); // For Create
        Task UpdateStoreAsync(Store store); // For Update
        Task ChangeStoreStatusAsync(Store store); // For Store Block
        Task DeleteStoreAsync(int id); // For Delete
        Task<Store> GetStoreAsync(int id); // For Read
        Task<IList<Store>> GetStoresAsync(); // For Read
        Task<(int total, int totalDisplay, IList<Store> records)> GetStoresAsync(int pageIndex,
            int pageSize, string searchText, string orderBy); //For Read
        Task<(IList<Store> stores, int storeCount)> GetStoreByUserIdAsync(Guid Id); //For Userwise store read
        Task<(int total, int totalDisplay, IList<Store> records)> GetStoresByUserIdAsync(
           Guid UserId, int pageIndex, int pageSize, string searchText, string orderBy);
        #endregion



        #region Non-Asynchronus Methods
        void CreateStore(Store store); // For Create
        void UpdateStore(Store store); // For Update 
        void DeleteStore(int id); // For Delete
        Store GetStore(int id); // For Read
        IList<Store> GetStores(); // For Read
        (int total, int totalDisplay, IList<Store> records) GetStores(int pageIndex, int pageSize,
            string searchText, string orderBy); //For Read
        #endregion
    }
}
