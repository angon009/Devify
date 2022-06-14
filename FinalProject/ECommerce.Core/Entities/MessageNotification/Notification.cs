using ECommerce.Core.Entities.Users;
using ECommerce.Data;

namespace ECommerce.Core.Entities.MessageNotification
{
    public class Notification : IEntity<int>
    {
        public int Id { get; set; }
        public NotificationType? MessageText { get; set; }
        public DateTime Date { get; set; }
        public string? Details { get; set; }
        //public Store? Store { get; set; }
        //public int? StoreId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ApplicationId { get; set; }

    }
}
