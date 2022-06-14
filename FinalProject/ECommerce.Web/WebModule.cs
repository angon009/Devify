using Autofac;
using ECommerce.Utility;
using ECommerce.Web.Areas.Admin.Models;
using ECommerce.Web.Areas.Profile.Models;
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels; 
using ECommerce.Web.Areas.Vendor.Models.ForMessageNotification;
using ECommerce.Web.Areas.Vendor.Models.OrdersModels;
using ECommerce.Web.Models;
using ECommerce.Web.Areas.StoreAdmin.Models.ProductModels; 
using ECommerce.Web.Areas.Vendor.Models.ProductModels;  
using ECommerce.Web.Areas.Vendor.Models.BillPayModels;  
using ECommerce.Web.Areas.Vendor.Models.ForDiscount; 
using ECommerce.Web.Areas.Vendor.Models.StockModels;
using ECommerce.Web.Areas.Vendor.Models.StoreModels;
using ECommerce.Web.Areas.Vendor.Models.SalesModels;
using ECommerce.Web.Areas.Vendor.Models.CustomerModels;

namespace ECommerce.Web
{
    public class WebModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpConfiguration _smtpConfiguration;

        public WebModule(IConfiguration configuration)
        {

            _configuration = configuration;

            var smtpSection = _configuration.GetSection("SmtpConfiguration");
            _smtpConfiguration = new SmtpConfiguration()
            {
                Server = smtpSection["Server"],
                Port = int.Parse(smtpSection["Port"]),
                Username = smtpSection["Username"],
                Password = smtpSection["Password"],
                UseSSL = bool.Parse(smtpSection["UseSSL"]),
                SenderName = smtpSection["SenderName"],
                SenderEmail = smtpSection["SenderEmail"]
            };
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryCreateViewModel>()
                .AsSelf();
            builder.RegisterType<CategoryListViewModel>()
                .AsSelf();
            builder.RegisterType<CategoryDeleteModel>()
                .AsSelf();
            builder.RegisterType<SubCategoryCreateModel>()
                .AsSelf();
            builder.RegisterType<SubCategoryListViewModel>()
                .AsSelf();

            builder.RegisterType<SubCategoryDeleteModel>()
                .AsSelf(); 
            builder.RegisterType<ProductCreateViewModel>()
                .AsSelf();  
            builder.RegisterType<EmailSender>().As<IEmailSender>()
                .WithParameter("smtpConfiguration", _smtpConfiguration)
                .InstancePerLifetimeScope(); 
            builder.RegisterType<SettingsModel>().AsSelf()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<StoreDetailsViewModel>()
                .AsSelf(); 
            builder.RegisterType<ProductDetailsModel>()
                .AsSelf();
            builder.RegisterType<ProductCreateViewModel>()
                .AsSelf();
            builder.RegisterType<ProductUpdateViewModel>()
               .AsSelf();
            builder.RegisterType<ProductListModel>()
                .AsSelf();
            builder.RegisterType<ProductListViewModel>()
                .AsSelf();
            builder.RegisterType<StockListViewModel>()
                .AsSelf();
            builder.RegisterType<StockUpdateModel>()
                .AsSelf();
            builder.RegisterType<StoreModel>()
                .AsSelf();
            builder.RegisterType<StoreListViewModel>()
                .AsSelf();
            builder.RegisterType<StoreUpdateModel>()
                .AsSelf();
            builder.RegisterType<MessageCreateModel>()
                .AsSelf();
            builder.RegisterType<MessageListModel>()
                .AsSelf();
            builder.RegisterType<StoreListModel>()
                .AsSelf();
            builder.RegisterType<PaymentListModel>()
                .AsSelf();
            builder.RegisterType<StoreStatusModel>()
                .AsSelf();
            builder.RegisterType<ShoppingCartModel>()
                .AsSelf(); 
            builder.RegisterType<CreateStorePaymentModel>()
                .AsSelf(); 
            builder.RegisterType<StorePaymentModel>()
                .AsSelf(); 
            builder.RegisterType<DiscountCreateModel>()
                .AsSelf();
            builder.RegisterType<DiscountUpdateModel>()
                .AsSelf(); 
            builder.RegisterType<DiscountListViewModel>()
                .AsSelf();  
            builder.RegisterType<OrderHistoryModel>()
                .AsSelf();
            builder.RegisterType<OrderListViewModel>()
                .AsSelf();
            builder.RegisterType<StoreStatusChangeModel>()
                .AsSelf();
            builder.RegisterType<SalesViewModel>()
                .AsSelf();
            builder.RegisterType<InventoryAlertCountModel>()
                .AsSelf();
            builder.RegisterType<CustomerListViewModel>()
                .AsSelf();

            base.Load(builder);
        }
    }
}
