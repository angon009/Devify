namespace ECommerce.Infrastructure.BusinessObjects.Stores
{
    public class StoreStatus
    {
        public int Id { get; set; }
        public List<Store>? Stores { get; set; }
        public string? Status { get; set; }
    }
}
