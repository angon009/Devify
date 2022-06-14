using ECommerce.Core.Entities.Products;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class StockDetail : IEntity<int>
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public Stock? Stock { get; set; }
        public int? StockId { get; set; }
    }
}
