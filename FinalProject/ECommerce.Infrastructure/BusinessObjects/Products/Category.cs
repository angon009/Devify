using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public int? StoreId { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}
