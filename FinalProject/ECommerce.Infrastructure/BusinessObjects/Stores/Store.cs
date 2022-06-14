using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.BusinessObjects.Stores
{
    public class Store
    {
        public int Id { get; set; }
        public string? StoreName { get; set; }
        public string? SubDomain { get; set; }
        public Email? Email { get; set; }
        public int? EmailId { get; set; }
        public Phone? Phone { get; set; }
        public int? PhoneId { get; set; }
        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        public List<Image>? StoreImages { get; set; }
        public List<Notification>? Notifications { get; set; }
        public StoreStatus StoreStatus { get; set; }
        public int? StoreStatusId { get; set; }
        public List<Product>? Products { get; set; }
        public Stock? Stock { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationUserId { get; set; }
    }
}
