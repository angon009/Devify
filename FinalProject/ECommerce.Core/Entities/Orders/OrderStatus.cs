using ECommerce.Data;

namespace ECommerce.Core.Entities.Orders
{
    public class OrderStatus : IEntity<int>
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
