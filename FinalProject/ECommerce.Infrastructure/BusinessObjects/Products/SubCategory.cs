namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string? SubCategoryName { get; set; }
        public List<Product>? Products { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
