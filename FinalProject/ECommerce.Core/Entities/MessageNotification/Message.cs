using ECommerce.Data;

namespace ECommerce.Core.Entities.MessageNotification
{
    public class Message : IEntity<int>
    {

        public int Id { get; set; }
        public string? MessageText { get; set; }
        public DateTime? Date { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }

    }
}
