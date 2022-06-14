using ECommerce.Core.Entities.Common;
using ECommerce.Core.Entities.MessageNotification;
using ECommerce.Core.Entities.Orders;
using ECommerce.Core.Entities.Products;
using ECommerce.Core.Entities.Stores;
using ECommerce.Core.Entities.Users;
using ECommerce.Core.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core.DbContexts
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser, Role, Guid, UserClaim
        , UserRole, UserLogin, RoleClaim, UserToken>, ICoreDbContext
    {
        private readonly string _connectionString;
        private readonly string _assemblyName;

        public CoreDbContext(string connectionString, string assemblyName)
        {
            _connectionString = connectionString;
            _assemblyName = assemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString, m => m.MigrationsAssembly(_assemblyName));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Data Seed
            modelBuilder.Entity<ApplicationUser>()
                .HasData(ApplicationUserSeed.Users);

            modelBuilder.Entity<StoreStatus>()
                .HasData(StoreStatusSeed.Status);

            modelBuilder.Entity<OrderStatus>()
                .HasData(OrderStatusSeed.Status);
            modelBuilder.Entity<Role>()
                .HasData(RoleSeed.Roles);
            modelBuilder.Entity<UserRole>()
                .HasData(UserRoleSeed.UserRole);
            #endregion


            modelBuilder.Entity<Product>()
                .HasMany(a=>a.ProductImages)
                .WithOne(i=>i.Product)
                .OnDelete(DeleteBehavior.Cascade);
            #region OneToManyRelationshipFluentAPI

            #region ApplicationUserOneToMany
            modelBuilder.Entity<ApplicationUser>()
                    .HasMany(a => a.Address)
                    .WithOne(u => u.ApplicationUser)
                    .HasForeignKey(fk => fk.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(s => s.Stores)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey(fk => fk.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.Carts)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey(fk => fk.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(n => n.Notifications)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey(fk => fk.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            //One Category Has Many Products
            modelBuilder.Entity<SubCategory>()
                .HasMany(p => p.Products)
                .WithOne(c => c.SubCategory)
                .HasForeignKey(fk => fk.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            //One Category Has many SubCategory
            modelBuilder.Entity<Category>()
                .HasMany(s => s.SubCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(fk => fk.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            //One Order Has Many OrderDetails
            modelBuilder.Entity<Order>()
                .HasMany(od => od.OrderDetails)
                .WithOne(o => o.Order)
                .HasForeignKey(fk => fk.OrderId);

            //One Store Has Many Products
            modelBuilder.Entity<Store>()
                .HasMany(p => p.Products)
                .WithOne(s => s.Store)
                .HasForeignKey(fk => fk.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion


            #region ManyToManyRelationshipFluentAPI
            //Many Category Has One Discount


            modelBuilder.Entity<Discount>()
                .HasMany(d => d.Categories)
                .WithOne(c => c.Discount)
                .HasForeignKey(fk => fk.DiscountId);


            //Many Product Has Many Color
            modelBuilder.Entity<ProductColor>().HasKey(c => new { c.ProductId, c.ColorId });

            modelBuilder.Entity<ProductColor>()
                .HasOne(p => p.Product)
                .WithMany(c => c.Colors)
                .HasForeignKey(fk => fk.ProductId);

            modelBuilder.Entity<ProductColor>()
                .HasOne(c => c.Color)
                .WithMany(p => p.Products)
                .HasForeignKey(fk => fk.ColorId);


            //Many Product Has Many Discount

            modelBuilder.Entity<Discount>()
                            .HasMany(d => d.Products)
                            .WithOne(c => c.Discount)
                            .HasForeignKey(fk => fk.DiscountId);
            #endregion

            base.OnModelCreating(modelBuilder);
        }



        #region CommonEntitiesDbSet
        public DbSet<Image> Images { get; set; }
        public DbSet<StorePayments> StorePayments { get; set; }
        #endregion


        #region MessageNotificationsDbSet
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        #endregion


        #region OrderEntitiesDbSet
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        #endregion


        #region ProductEntitiesDbSet
        public DbSet<InventoryAlert> InventoryAlerts { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<CategoryDiscount> CategoryDiscounts { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        //public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        #endregion


        #region StoreEntities
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreStatus> StoreStatuses { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        #endregion 
    }
}
