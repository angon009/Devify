using ECommerce.Core.DbContexts;
using ECommerce.Data;
using ECommerce.Infrastructure.Repositories.ForAddress;
using ECommerce.Infrastructure.Repositories.ForCategory;
using ECommerce.Infrastructure.Repositories.ForMessageNotification;
using ECommerce.Infrastructure.Repositories.ForProduct;
using ECommerce.Infrastructure.Repositories.ForProductColor;
using ECommerce.Infrastructure.Repositories.ForStock;
using ECommerce.Infrastructure.Repositories.ForStore;
using ECommerce.Infrastructure.Repositories.ForSubCategory;
using Microsoft.EntityFrameworkCore; 
using ECommerce.Infrastructure.Repositories.ForCart;
using ECommerce.Infrastructure.Repositories.ForOrder;  
using ECommerce.Infrastructure.Repositories.ForStorePayment; 

namespace ECommerce.Infrastructure.UnitOfWorks
{
    public class EcommerceUnitOfWork : UnitOfWork, IEcommerceUnitOfWork
    {

        public ISubCategoryRepository SubCategories { get; private set; }
        public IStoreRepository Stores { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IStockRepository Stocks { get; private set; }
        public IProductColorRepository ProductColors { get; private set; }
        public IAddressRepository Addresses { get; set; }
        public IMessageRepository Messages { get; set; } 
        public ICartRepository Carts { get; set; }
        public IOrderRepository Orders { get; private set; }
        public IImageRepository Images { get; set; }
        public IDiscountRepository Discounts { get; set; } 
        public IStorePaymentRepository StorePayments { get; set; } 
        public IInventoryAlertRepository InventoryAlerts { get; set; }  

        public EcommerceUnitOfWork(ICoreDbContext dbContext,
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository,
            IStoreRepository storeRepository,
            IProductRepository productRepository,
            IStockRepository stockRepository,
            IProductColorRepository productColorRepository,
            IAddressRepository addressRepository,
            IMessageRepository messageRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IImageRepository imageRepository,
            IDiscountRepository discountRepository,
            IStorePaymentRepository storePaymentRepository,
            IInventoryAlertRepository inventoryAlerts)
            : base((DbContext)dbContext)
        {
            SubCategories = subCategoryRepository;
            Categories = categoryRepository;
            Stores = storeRepository;
            Products = productRepository;
            Stocks = stockRepository;
            ProductColors = productColorRepository;
            Addresses = addressRepository;
            Messages = messageRepository;
            Carts = cartRepository;
            Orders = orderRepository;
            Images = imageRepository;
            Discounts = discountRepository;
            ProductColors = productColorRepository;
            Addresses = addressRepository;
            Messages = messageRepository;
            StorePayments = storePaymentRepository;
            InventoryAlerts = inventoryAlerts;
        }
    }
}
