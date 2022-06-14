using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class StoreStatus : IEntity<int>
    {
        public int Id { get; set; }
        public List<Store>? Stores { get; set; }
        public string? Status { get; set; }
    }
}
