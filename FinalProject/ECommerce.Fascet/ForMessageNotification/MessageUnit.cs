using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Infrastructure.Services.ForMessageNotification;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForMessageNotification
{
    internal class MessageUnit : IMessageUnit
    {
        private IMessageService _messageService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public MessageUnit(IEcommerceUnitOfWork ecommerceUnitOfWork, IMessageService messageService)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _messageService = messageService;
        }
        public async Task CreateServiceAsync(Message item)
        {
            await _messageService.CreateMessageAsync(item);
            await _ecommerceUnitOfWork.SaveAsync();
        }
        public Task DeleteServiceAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task UpdateServiceAsync(Message item)
        {
            throw new NotImplementedException();
        }
        public void CreateService(Message item)
        {
            throw new NotImplementedException();
        }
        public void UpdateService(Message item)
        {
            throw new NotImplementedException();
        }
        public void DeleteService(int id)
        {
            throw new NotImplementedException();
        }
    }
}
