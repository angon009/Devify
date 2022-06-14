using ECommerce.Core.Entities.Common;
using ECommerce.Core.Entities.MessageNotification;
using ECommerce.Core.Entities.Products;
using ECommerce.Core.Entities.Users;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class Store : IEntity<int>
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
        public StoreStatus? StoreStatus { get; set; }
        public int? StoreStatusId { get; set; }
        public List<Product>? Products { get; set; }
        public Stock? Stock { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationUserId { get; set; }

    }
}
