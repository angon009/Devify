using ECommerce.Data;

namespace ECommerce.Core.Entities.Products
{
    public class ProductColor : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public Product? Product { get; set; }
        public Color? Color { get; set; }

    }
}
