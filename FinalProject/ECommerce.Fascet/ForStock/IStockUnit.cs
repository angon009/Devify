using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Fascet.ForStock
{
    public interface IStockUnit : IUnit<Stock>
    {
        Task UpdateQuantityServiceAsync(int id, int quantity);
    }
}
