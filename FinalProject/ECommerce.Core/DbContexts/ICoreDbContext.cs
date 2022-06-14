using ECommerce.Core.Entities.Common;
using ECommerce.Core.Entities.MessageNotification;
using ECommerce.Core.Entities.Orders;
using ECommerce.Core.Entities.Products;
using ECommerce.Core.Entities.Stores;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core.DbContexts
{
    public interface ICoreDbContext
    {
        #region CommonEntitiesDbSet
        DbSet<Image> Images { get; set; }
        DbSet<StorePayments> StorePayments { get; set; }
        #endregion

        #region MessageNotificationsDbSet
        DbSet<Message> Messages { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<NotificationType> NotificationTypes { get; set; }
        #endregion

        #region OrderEntitiesDbSet
        DbSet<Cart> Carts { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<OrderStatus> OrderStatuses { get; set; }
        #endregion

        #region ProductEntitiesDbSet
        DbSet<Category> Categories { get; set; }
        DbSet<Color> Colors { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductColor> ProductColors { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        #endregion

        #region StoreEntities
        DbSet<Address> Addresses { get; set; }
        DbSet<Email> Emails { get; set; }
        DbSet<Phone> Phones { get; set; }
        DbSet<Stock> Stocks { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<StoreStatus> StoreStatuses { get; set; }
        DbSet<StockDetail> StockDetails { get; set; }
        #endregion
    }
}
