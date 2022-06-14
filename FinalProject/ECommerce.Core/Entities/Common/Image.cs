using ECommerce.Core.Entities.Products;
using ECommerce.Core.Entities.Stores;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Common
{
    public class Image : IEntity<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? ImageFor { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public int? StoreId { get; set; }
        public Store? Store { get; set; }
    }
}
