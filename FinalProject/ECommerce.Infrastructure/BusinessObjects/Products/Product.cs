using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    [Serializable]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public string? Weight { get; set; }
        public string? Size { get; set; }
        public int? Quantity { get; set; }
        public string? ProductDetails { get; set; }
        public List<Image>? ProductImages { get; set; }
        public List<ProductColor>? Colors { get; set; }
        public string? Color { get; set; }
        public SubCategory? SubCategory { get; set; }
        public int? SubCategoryId { get; set; }
        public Store? Store { get; set; }
        public int? StoreId { get; set; }
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }

    }
}
