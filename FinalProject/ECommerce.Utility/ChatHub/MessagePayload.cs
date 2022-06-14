using ECommerce.Core.Entities.Users;

namespace ECommerce.Utility.ChatHub
{
    public class MessagePayload
    {
        public int Id { get; set; }
        public string? MessageText { get; set; }
        public DateTime Date { get; set; }
        public ApplicationUser? Sender { get; set; }
        public ApplicationUser? Receiver { get; set; }
        public Guid? ApplicationUserId { get; set; }
    }
}
