using AutoMapper;
using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using MessageEntity = ECommerce.Core.Entities.MessageNotification.Message;

namespace ECommerce.Infrastructure.Services.ForMessageNotification
{
    public class MessageService : IMessageService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<MessageService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task CreateMessageAsync(Message message)
        {
            try
            {
                var entity = _mapper.Map<MessageEntity>(message);
                await _ecommerceUnitOfWork.Messages.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<IList<Message>> GetMessagesAsync(string Sender, string Reciever)
        {
            try
            {
                var messageEntities = (await _ecommerceUnitOfWork.Messages
                        .GetAsync(x => x.SenderEmail == Sender && x.ReceiverEmail == Reciever
                        || x.SenderEmail == Reciever && x.ReceiverEmail == Sender, string.Empty))
                        .OrderBy(x => x.Date);
                List<Message> messages = new List<Message>();
                foreach (var messageEntity in messageEntities)
                {
                    var message = _mapper.Map<Message>(messageEntity);
                    messages.Add(message);
                }
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<IList<Message>> GetMessageSendersAsync(string Receiver)
        {
            try
            {
                var messageEntities = (await _ecommerceUnitOfWork.Messages
                        .GetAsync(x => x.ReceiverEmail == Receiver, string.Empty))
                        .OrderBy(x => x.Date).DistinctBy(x => x.SenderEmail);

                List<Message> messages = new List<Message>();
                foreach (var messageEntity in messageEntities)
                {
                    var message = _mapper.Map<Message>(messageEntity);
                    messages.Add(message);
                }
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
