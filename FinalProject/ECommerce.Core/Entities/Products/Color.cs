using ECommerce.Data;

namespace ECommerce.Core.Entities.Products
{
    public class Color : IEntity<int>
    {
        public int Id { get; set; }
        public string? ColorName { get; set; }
        public List<ProductColor>? Products { get; set; }
    }
}
