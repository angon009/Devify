using ECommerce.Data;
using ECommerce.Infrastructure.Repositories.ForAddress;
using ECommerce.Infrastructure.Repositories.ForCategory;
using ECommerce.Infrastructure.Repositories.ForMessageNotification;
using ECommerce.Infrastructure.Repositories.ForProduct;
using ECommerce.Infrastructure.Repositories.ForProductColor;
using ECommerce.Infrastructure.Repositories.ForStock;
using ECommerce.Infrastructure.Repositories.ForStore;
using ECommerce.Infrastructure.Repositories.ForSubCategory; 
using ECommerce.Infrastructure.Repositories.ForCart;
using ECommerce.Infrastructure.Repositories.ForOrder;
using ECommerce.Infrastructure.Repositories.ForStorePayment;

namespace ECommerce.Infrastructure.UnitOfWorks
{
    public interface IEcommerceUnitOfWork : IUnitOfWork
    {
        ISubCategoryRepository SubCategories { get; }
        IStoreRepository Stores { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IStockRepository Stocks { get; } 
        IProductColorRepository ProductColors { get; }
        IAddressRepository Addresses { get; }
        IMessageRepository Messages { get; set; } 
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IImageRepository Images { get; }
        IDiscountRepository Discounts { get; }  
        IStorePaymentRepository StorePayments { get; set; } 
        IInventoryAlertRepository InventoryAlerts { get; set; }
    }
}
