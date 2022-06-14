using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForMessageNotification;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;

namespace ECommerce.Web.Areas.Vendor.Models.ForMessageNotification
{
    public class MessageCreateModel
    {
        public int Id { get; set; }
        public string? MessageText { get; set; }
        public DateTime? Date { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }
        private IMessageUnit? _messageUnit;
        private IMapper? _mapper;
        private ILifetimeScope? _scope;
        public MessageCreateModel()
        {

        }

        public MessageCreateModel(IMapper mapper, IMessageUnit messageUnit)
        {
            _messageUnit = messageUnit;
            _mapper = mapper;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _messageUnit = scope.Resolve<IMessageUnit>();
            _mapper = scope.Resolve<IMapper>();
        }

        internal async Task CreateMessage()
        {
            Message message = new Message()
            {
                MessageText = this.MessageText,
                Date=this.Date,
                SenderName= this.SenderName,
                ReceiverName=this.ReceiverName,
                SenderEmail=this.SenderEmail,
                ReceiverEmail=this.ReceiverEmail
            };
             
            await _messageUnit!.CreateServiceAsync(message);
        }
    }
}

