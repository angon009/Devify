using Autofac;
using ECommerce.Infrastructure.Repositories.ForAddress;
using ECommerce.Infrastructure.Repositories.ForCategory;
using ECommerce.Infrastructure.Repositories.ForMessageNotification;
using ECommerce.Infrastructure.Repositories.ForProduct;
using ECommerce.Infrastructure.Repositories.ForProductColor;
using ECommerce.Infrastructure.Repositories.ForStock;
using ECommerce.Infrastructure.Repositories.ForStore;
using ECommerce.Infrastructure.Repositories.ForSubCategory;
using ECommerce.Infrastructure.Services.ForAddress;
using ECommerce.Infrastructure.Services.ForCategory;
using ECommerce.Infrastructure.Services.ForMessageNotification;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.Services.ForStock;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Infrastructure.Services.ForSubCategory;
using ECommerce.Infrastructure.UnitOfWorks; 
using ECommerce.Infrastructure.Repositories.ForCart;
using ECommerce.Infrastructure.Services.ForCart;
using ECommerce.Infrastructure.Repositories.ForOrder;
using ECommerce.Infrastructure.Services.ForOrder;
using ECommerce.Infrastructure.Repositories.ForStorePayment;
using ECommerce.Infrastructure.Services.ForStorePayments;

namespace ECommerce.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _assemblyName;


        public InfrastructureModule(string connectionString, string assemblyName)
        {
            _connectionString = connectionString;
            _assemblyName = assemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Autofac Bindings should be here. 

            #region Bindings

            builder.RegisterType<EcommerceUnitOfWork>().As<IEcommerceUnitOfWork>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<SubCategoryRepository>().As<ISubCategoryRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<SubCategoryService>().As<ISubCategoryService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ProductService>().As<IProductService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StoreRepository>().As<IStoreRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StoreService>().As<IStoreService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CategoryService>().As<ICategoryService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StockRepository>().As<IStockRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StockService>().As<IStockService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ProductColorRepository>().As<IProductColorRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<AddressRepository>().As<IAddressRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<AddressService>().As<IAddressService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>()
                 .InstancePerLifetimeScope();
            builder.RegisterType<CartRepository>().As<ICartRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CartService>().As<ICartService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<OrderRepository>().As<IOrderRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<OrderService>().As<IOrderService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ImageRepository>().As<IImageRepository>()
               .InstancePerLifetimeScope();  
            builder.RegisterType<DiscountService>().As<IDiscountService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<DiscountRepository>().As<IDiscountRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StorePaymentRepository>().As<IStorePaymentRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<StorePaymentService>().As<IStorePaymentService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<InventoryAlertService>().As<IInventoryAlertService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<InventoryAlertRepository>().As<IInventoryAlertRepository>()
                .InstancePerLifetimeScope();
            #endregion

            base.Load(builder);
        }
    }
}
