using Autofac;
using ECommerce.Fascet.ForCategory;
using ECommerce.Fascet.ForDiscount;
using ECommerce.Fascet.ForMessageNotification;
using ECommerce.Fascet.ForProduct;
using ECommerce.Fascet.ForStock;
using ECommerce.Fascet.ForStore;
using ECommerce.Fascet.ForStorePayment;
using ECommerce.Fascet.ForSubCategory;

namespace ECommerce.Fascet
{
    public class FascetModule : Module
    {


        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryUnit>().As<ICategoryUnit>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SubCategoryUnit>().As<ISubCategoryUnit>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StoreUnit>().As<IStoreUnit>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductUnit>().As<IProductUnit>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MessageUnit>().As<IMessageUnit>()
               .InstancePerLifetimeScope();

            builder.RegisterType<DiscountUnit>().As<IDiscountUnit>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StorePaymentUnit>().As<IStorePaymentUnit>();

            builder.RegisterType<StockUnit>().As<IStockUnit>()
                .InstancePerLifetimeScope();
            builder.RegisterType<InventoryAlertUnit>().As<IInventoryAlertUnit>()
                .InstancePerLifetimeScope();




            base.Load(builder);
        }

    }
}
