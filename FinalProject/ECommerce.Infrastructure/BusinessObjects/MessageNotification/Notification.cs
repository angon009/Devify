using ECommerce.Infrastructure.BusinessObjects.Stores;

namespace ECommerce.Infrastructure.BusinessObjects.MessageNotification
{
    public class Notification
    {
        public int Id { get; set; }
        public NotificationType? MessageText { get; set; }
        public DateTime Date { get; set; }
        public string? Details { get; set; }
        public Store? Store { get; set; }
        public int StoreId { get; set; }
    }
}
