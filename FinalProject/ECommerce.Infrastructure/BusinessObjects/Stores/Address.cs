namespace ECommerce.Infrastructure.BusinessObjects.Stores
{
    public class Address
    {
        public int Id { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public string? Thana { get; set; }
        public string? PostOffice { get; set; }
        public string? RoadNumber { get; set; }
    }
}
