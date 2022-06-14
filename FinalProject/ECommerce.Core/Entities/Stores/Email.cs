using ECommerce.Data;

namespace ECommerce.Core.Entities.Stores
{
    public class Email : IEntity<int>
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; }
    }
}
