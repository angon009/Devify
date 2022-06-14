namespace ECommerce.Infrastructure.BusinessObjects.Common
{
    public class Image
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? ImageFor { get; set; }
        public int? ProductId { get; set; }
    }
}
