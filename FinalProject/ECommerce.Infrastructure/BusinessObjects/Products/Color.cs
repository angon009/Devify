namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    public class Color
    {
        public int Id { get; set; }
        public string? ColorName { get; set; }
        public List<ProductColor>? Products { get; set; }
    }
}
