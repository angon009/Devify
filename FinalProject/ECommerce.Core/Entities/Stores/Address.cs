using ECommerce.Core.Entities.Users;
using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class Address : IEntity<int>
    {
        public int Id { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public string? Thana { get; set; }
        public string? PostOffice { get; set; }
        public string? RoadNumber { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationId { get; set; }
    }
}
