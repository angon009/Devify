using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Fascet.ForStore
{
    public interface IStoreUnit : IUnit<Store>
    {
        Task<(IList<Store> stores, int storeCount)> GetServiceAsync(Guid Id);
        Task<(int total, int totalDisplay, IList<Store> records)> GetServiceAsync(
            int pageIndex, int pageSize, string searchText, string orderBy);


        (int total, int totalDisplay, IList<Store> records) GetService(
            int pageIndex, int pageSize, string searchText, string orderBy);
        //void SaveService(T item);
        Task StoreDisableServiceAsync(int id);
        Task StoreEnableServiceAsync(int id);
        Task StoreOnDeleteServiceAsync(int id);
    }
}
