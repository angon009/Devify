using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class Stock : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime StockEntryDate { get; set; }
        public Store? Store { get; set; }
        public int? StoreId { get; set; }
        public List<StockDetail>? StockDetails { get; set; }
    }
}
