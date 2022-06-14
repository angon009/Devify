using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Utility;
using Newtonsoft.Json;

namespace ECommerce.Web.Models
{
    public class CartItemModel
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? DiscountId { get; set; }
        public double TotalPrice { get; set; } // SalePrice*Quantity
        public double TotalCostPrice { get; set; } // CostPrice*Quantity
        public double? DiscountTotal { get; set; } // (SalePrice/100*Percentage)*Quantity
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationUserId { get; set; }
    }
}
