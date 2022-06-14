using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.BusinessObjects.Orders
{
    public class OrderDetail
    {
        public int Id { get; set; }
        //public Discount? Discount { get; set; }
        public int DiscountId { get; set; }
        public int? Quantity { get; set; }
        public Order? Order { get; set; }
        public int? OrderId { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
        public Color? Color { get; set; }
        public int? ColorId { get; set; }
    }
}
