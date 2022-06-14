using Autofac;
using AutoMapper;
using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForOrder;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.StoreAdmin.Models.StoreModels
{
    public class StoreDetailsViewModel
    {
        private IStoreService _storeService;
        private ILifetimeScope _scope;
        private IMapper _mapper;
        private IAccountRepository _accountRepo;
        private IOrderService _orderService;
        public StoreDetailsViewModel()
        {

        }

        public StoreDetailsViewModel(IMapper mapper, IStoreService storeService, IAccountRepository accountRepo, IOrderService orderService)
        {

            _mapper = mapper;
            _storeService = storeService;
            _accountRepo = accountRepo;
            _orderService = orderService;
        }



        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _storeService = _scope.Resolve<IStoreService>();
            _accountRepo = _scope.Resolve<IAccountRepository>();
            _mapper = _scope.Resolve<IMapper>();
        }
        public int Id { get; set; }

        [Display(Name = "Store Name")]
        [Required(ErrorMessage = "Store Name Can't Be Null")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Email Can't Be Null")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Can't Be Null")]
        public string Phone { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }
        [Display(Name = "District")]
        public string District { get; set; }
        [Display(Name = "Thana")]
        public string Thana { get; set; }
        [Display(Name = "RoadNo")]
        public string RoadNo { get; set; }
        [Display(Name = "Post Office")]
        public string post { get; set; }

        [Display(Name = "Store Status")]
        public string StoreStatus { get; set; }
        public string CoverPhotUrl { get; set; }
        public string LogoUrl { get; set; }
        public int? EmailId { get; set; }
        public int? PhoneId { get; set; }
        public int? AddressId { get; set; }
        public Guid UserId { get; set; }
        public int? CoverImageId { get; set; }
        public int? LogoImageId { get; set; }
        public int StoreStatusId { get; set; }
        public string CoverImageName { get; set; }
        public string LogoImageName { get; set; }
        public ApplicationUser User { get; set; } 
        public string SubDomain { get; set; } 

        public async Task<StoreDetailsViewModel> GetStoreDetails(int Id)
        {
            Store store = await _storeService.GetStoreAsync(Id);
            var CoverImages = store.StoreImages!.Where(x => x.ImageFor == "CoverImage").ToList();
            var LogoImages = store.StoreImages!.Where(x => x.ImageFor == "LogoImage").ToList();

            this.Id = store.Id;
            StoreName = store.StoreName!;
            Email = store.Email!.EmailAddress!;
            Phone = store.Phone!.PhoneNumber!;
            Division = store.Address!.Division!;
            District = store.Address!.District!;
            Thana = store.Address!.Thana!;
            post = store.Address!.PostOffice!;
            RoadNo = store.Address!.RoadNumber!;
            StoreStatus = StoreStatus;
            SubDomain = store.SubDomain!;
            EmailId = store.EmailId;
            AddressId = store.AddressId;
            PhoneId = store.PhoneId;
            UserId = (Guid)store!.ApplicationUserId!;
            StoreStatusId = (int)store!.StoreStatusId!;

            if (CoverImages!.Count > 0)
            {
                CoverPhotUrl = CoverImages.Count > 0 ? CoverImages[0].Url! : null!;
                CoverImageId = CoverImages.Count > 0 ? CoverImages[0].Id! : null!;
                CoverImageName = CoverImages.Count > 0 ? CoverImages[0].Name! : null!;

            };
            if (LogoImages!.Count > 0)
            {
                LogoUrl = LogoImages.Count > 0 ? LogoImages[0].Url! : null!;
                LogoImageId = CoverImages.Count > 0 ? LogoImages[0].Id! : null!;
                LogoImageName = CoverImages.Count > 0 ? LogoImages[0].Name! : null!;
            }

            return this;
        }
        public async Task<StoreDetailsViewModel> GetStoreBySubDomainAsync()
        {
            Store store = await _storeService.GetStoreBySubDomainAsync(UrlAction.GetSubDomain());
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
            storeModel.UserId = (Guid)store.ApplicationUserId;
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

        public async Task<object> GetLatestOrders(int StoreId)
        {
            var user = await _accountRepo.GetCurrentUserAsync();
            var data = await _orderService.GetOrdersAsync(
                StoreId,
                1,
                10,
                string.Empty,
                "OrderDate");

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Id.ToString(),
                                record!.ApplicationUser.FirstName!,
                                record.ApplicationUser.Email,
                                record.TotalAmount.ToString(),
                                record.DiscountTotal.ToString(),
                                (record.OrderStatusId==1)?"Pending":(record.OrderStatusId==2)?
                                "Ondelivery":(record.OrderStatusId==3)?"Delivered":"Cancelled",
                                record!.OrderDate!.CurrentZone(user.TimeZone!).ToString()!                                
                        }
                    ).ToArray()
            };
        }
    }
}
