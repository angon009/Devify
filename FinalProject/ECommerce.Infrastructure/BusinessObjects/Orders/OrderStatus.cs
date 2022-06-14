namespace ECommerce.Infrastructure.BusinessObjects.Orders
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
