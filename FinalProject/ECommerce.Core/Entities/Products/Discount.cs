using ECommerce.Data;

namespace ECommerce.Core.Entities.Products
{
    public class Discount : IEntity<int>
    {
        public int Id { get; set; }
        public string? DiscountName { get; set; }
        public int? Percentage { get; set; }
        public double? Amount { get; set; }
        public string? Details { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int StoreId { get; set; }
        public List<Product>? Products { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
