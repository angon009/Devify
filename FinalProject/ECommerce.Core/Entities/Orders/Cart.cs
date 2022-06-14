using ECommerce.Core.Entities.Products;
using ECommerce.Core.Entities.Users;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Orders
{
    public class Cart : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? DiscountId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationUserId { get; set; }
    }
}
