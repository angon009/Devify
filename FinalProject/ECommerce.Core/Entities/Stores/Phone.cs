using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class Phone : IEntity<int>
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
