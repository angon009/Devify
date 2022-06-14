using ECommerce.Data;

namespace ECommerce.Core.Entities.Products
{
    public class SubCategory : IEntity<int>
    {
        public int Id { get; set; }
        public string? SubCategoryName { get; set; }
        public List<Product>? Products { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }

    }
}
