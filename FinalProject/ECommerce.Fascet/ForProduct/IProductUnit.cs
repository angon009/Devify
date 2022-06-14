using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Fascet.ForProduct
{
    public interface IProductUnit : IUnit<Product>
    {
        Task UpdateQuantityServiceAsync(int id, int quantity);
        Task IncreaseQuantityServiceAsync(int id, int quantity);
        Task DecreaseQuantityServiceAsync(int id, int quantity);
    }
}