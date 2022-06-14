using ECommerce.Core.Entities.Stores;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Products
{
    public class Category : IEntity<int>
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
