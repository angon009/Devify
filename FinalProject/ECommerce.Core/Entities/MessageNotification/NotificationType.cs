using ECommerce.Data;

namespace ECommerce.Core.Entities.MessageNotification
{
    public class NotificationType : IEntity<int>
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
    }
}
