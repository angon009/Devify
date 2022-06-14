using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.BusinessObjects.Stores
{
    public class StockDetail
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public Stock? Stock { get; set; }
        public int? StockId { get; set; }
    }
}
