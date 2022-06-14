namespace ECommerce.Fascet
{
    public interface IUnit<T>
    {
        Task CreateServiceAsync(T item);
        Task UpdateServiceAsync(T item);
        Task DeleteServiceAsync(int id);
        void CreateService(T item);

        void UpdateService(T item);
        void DeleteService(int id);
    }
}
