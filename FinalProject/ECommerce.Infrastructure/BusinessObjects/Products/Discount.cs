namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    public class Discount
    {
        public int Id { get; set; }
        public string? DiscountName { get; set; }
        public int? Percentage { get; set; }
        public double? Amount { get; set; }
        public string? Details { get; set; }
        public int StoreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        //public List<Product>? Products { get; set; }
        //public List<Category>? Categories { get; set; }
    }
}
