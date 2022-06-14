namespace ECommerce.Core.StoredProcedureEntites
{
    public class FilteredProducts
    {
        public FilteredProducts()
        {

        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Weight { get; set; }
        public string? Size { get; set; }
        public double? SalePrice { get; set; }
        public string? Details { get; set; }
        public string? ImageName { get; set; }
        public int? DiscountId { get; set; }
        public int? Percentage { get; set; }
    }
}
