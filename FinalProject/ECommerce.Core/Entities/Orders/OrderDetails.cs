using ECommerce.Core.Entities.Products;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Orders
{
    public class OrderDetail : IEntity<int>
    {
        public int Id { get; set; }
        //public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }
        public int? Quantity { get; set; }
        public Order? Order { get; set; }
        public int? OrderId { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
        public Color? Color { get; set; }
        public int? ColorId { get; set; }
    }
}