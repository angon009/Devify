using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.BusinessObjects.Orders
{
    public class Cart
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
