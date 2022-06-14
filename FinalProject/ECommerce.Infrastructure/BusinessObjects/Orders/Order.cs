using ECommerce.Core.Entities.Users;

namespace ECommerce.Infrastructure.BusinessObjects.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderDescription { get; set; }
        public double TotalAmount { get; set; }
        public double TotalCostAmount { get; set; }
        public double DiscountTotal { get; set; }
        public int? StoreId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
