using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.MessageNotification;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.ForMessageNotification
{
    internal class MessageRepository : Repository<Message, int>, IMessageRepository
    {
        public MessageRepository(ICoreDbContext context) : base((DbContext)context)
        {

        }
    }
}
