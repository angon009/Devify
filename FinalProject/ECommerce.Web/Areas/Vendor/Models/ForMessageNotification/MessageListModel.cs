using Autofac;
using AutoMapper;
using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForMessageNotification;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;

namespace ECommerce.Web.Areas.Vendor.Models.ForMessageNotification
{
    public class MessageListModel
    {
        public int Id { get; set; }
        public string? MessageText { get; set; }
        public DateTime? Date { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public IList<MessageListModel>? SendersMessages { get; set; }

        private IMessageService? _messageService;
        private IMapper? _mapper;
        private ILifetimeScope? _scope;
        private IAccountRepository _accountRepo;
        public IStoreService _storeService;
        public MessageListModel()
        {

        }

        public MessageListModel(IMapper mapper, IMessageService messageService,
            IAccountRepository accountRepo,IStoreService storeService)
        {
            _messageService = messageService;
            _mapper = mapper;
            _accountRepo = accountRepo;
            _storeService = storeService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _messageService = scope.Resolve<IMessageService>();
            _mapper = scope.Resolve<IMapper>();
            _storeService=scope.Resolve<IStoreService>();
            _accountRepo=scope.Resolve<IAccountRepository>();
        }

        public async Task<StoreDetailsViewModel> GetStoreBySubDomainAsync()
        {
            string subDomain=UrlAction.GetSubDomain();
            Store store=await _storeService.GetStoreBySubDomainAsync(subDomain);
            var CoverImages = store.StoreImages!.Where(x => x.ImageFor == "CoverImage").ToList();
            var LogoImages = store.StoreImages!.Where(x => x.ImageFor == "LogoImage").ToList();
            StoreDetailsViewModel storeModel = _scope.Resolve<StoreDetailsViewModel>();
            storeModel.Id = store.Id;
            storeModel.StoreName = store.StoreName!;
            storeModel.Email = store.Email!.EmailAddress!;
            storeModel.Phone = store.Phone!.PhoneNumber!;
            storeModel.Division = store.Address!.Division!;
            storeModel.District = store.Address!.District!;
            storeModel.Thana = store.Address!.Thana!;
            storeModel.post = store.Address!.PostOffice!;
            storeModel.RoadNo = store.Address!.RoadNumber!;
            storeModel.StoreStatus = store.StoreStatus.Status;
            storeModel.EmailId = store.EmailId;
            storeModel.AddressId = store.AddressId;
            storeModel.PhoneId = store.PhoneId;
            storeModel.UserId = (Guid)store.ApplicationUserId;
            storeModel.StoreStatusId = (int)store.StoreStatusId;
            storeModel.UserId= (Guid)store.ApplicationUserId;
            storeModel.User = store.ApplicationUser;

            if (CoverImages!.Count > 0)
            {
                storeModel.CoverPhotUrl = CoverImages.Count > 0 ? CoverImages[0].Url! : null!;
                storeModel.CoverImageId = CoverImages.Count > 0 ? CoverImages[0].Id! : null!;
                storeModel.CoverImageName = CoverImages.Count > 0 ? CoverImages[0].Name! : null!;

            };
            if (LogoImages!.Count > 0)
            {
                storeModel.LogoUrl = LogoImages.Count > 0 ? LogoImages[0].Url! : null!;
                storeModel.LogoImageId = CoverImages.Count > 0 ? LogoImages[0].Id! : null!;
                storeModel.LogoImageName = CoverImages.Count > 0 ? LogoImages[0].Name! : null!;
            }

            return storeModel;
        }
        internal async Task<List<MessageListModel>> GetMessages()
        {
            var messages = await _messageService!.GetMessagesAsync(this.SenderEmail, this.ReceiverEmail);
            var messagesList = new List<MessageListModel>();
            foreach (var message in messages)
            {
                var messageModel = _mapper!.Map<MessageListModel>(message);
                messagesList.Add(messageModel);
            }
            return messagesList;
        }
       internal async Task <List<MessageListModel>> GetMessageSendersAsync()
       {
            var messages = await _messageService!.GetMessageSendersAsync(this.SenderEmail);
            var messagesList = new List<MessageListModel>();
            foreach (var message in messages)
            {
                var messageModel = _mapper!.Map<MessageListModel>(message);
                messagesList.Add(messageModel);
            }
            this.SendersMessages = messagesList;
            return messagesList;
        }
    }
}

