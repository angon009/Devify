namespace ECommerce.Infrastructure.BusinessObjects.Stores
{
    public class Stock
    {
        public int Id { get; set; }
        public DateTime StockEntryDate { get; set; }
        public Store? Store { get; set; }
        public int? StoreId { get; set; }
        public List<StockDetail>? StockDetails { get; set; }
    }
}
