using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;

namespace ECommerce.Infrastructure.Services.ForMessageNotification
{
    public interface IMessageService
    {
        Task CreateMessageAsync(Message message); // For Create
        Task<IList<Message>> GetMessagesAsync(string Sender, string Reciever); // For Read
        Task<IList<Message>> GetMessageSendersAsync(string Receiver);
    }
}
