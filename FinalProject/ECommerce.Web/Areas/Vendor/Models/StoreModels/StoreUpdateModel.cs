using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForStore;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Membership.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.StoreAdmin.Models.StoreModels
{
    public class StoreUpdateModel
    {
        private IStoreService _storeService;
        private IStoreUnit _storeUnit;
        private ILifetimeScope _scope;
        private IMapper _mapper;
        private IAccountRepository _accountRepo;
        private IWebHostEnvironment _webHostEnvironment;
        public StoreUpdateModel()
        {

        }

        public StoreUpdateModel(IMapper mapper, IStoreService storeService,
            IStoreUnit storeUnit, IAccountRepository accountRepo, IWebHostEnvironment webHostEnvironment)
        {

            _mapper = mapper;
            _storeService = storeService;
            _storeUnit = storeUnit;
            _accountRepo = accountRepo;
            _webHostEnvironment = webHostEnvironment;
        }



        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _webHostEnvironment = _scope.Resolve<IWebHostEnvironment>();
            _storeService = _scope.Resolve<IStoreService>();
            _storeUnit = _scope.Resolve<IStoreUnit>();
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
        public string? Division { get; set; }
        [Display(Name = "District")]
        public string? District { get; set; }
        [Display(Name = "Thana")]
        public string? Thana { get; set; }
        [Display(Name = "RoadNo")]
        public string? RoadNo { get; set; }
        [Display(Name = "Post Office")]
        public string? post { get; set; }

        [Display(Name = "Store Status")]
        public string? StoreStatus { get; set; }
        public int EmailId { get; set; }
        public int PhoneId { get; set; }
        public int AddressId { get; set; }
        public Guid UserId { get; set; }
        public int? CoverImageId { get; set; }
        public int? LogoImageId { get; set; }
        public int StoreStatusId { get; set; }
        public IFormFile? CoverPhoto { get; set; }
        public IFormFile? StoreLogo { get; set; }
        public string? CoverImageName { get; set; }
        public string? LogoImageName { get; set; }
        public string? CoverPhotUrl { get; set; }
        public string? LogoUrl { get; set; }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            var fileUrl = Guid.NewGuid().ToString() + "_" + file.FileName;
            folderPath += fileUrl;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));



            return fileUrl;
        }
        private async Task DeleteImage(string folderPath)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task UpdateStore()
        {
            if (CoverPhoto != null)
            {
                await DeleteImage("Images/Stores/" + CoverPhotUrl);
                CoverPhotUrl = await UploadImage("Images/Stores/", CoverPhoto);
                CoverImageName = CoverPhoto.FileName;
            }
            if (StoreLogo != null)
            {
                await DeleteImage("Images/Stores/" + LogoUrl);
                LogoUrl = await UploadImage("Images/Stores/", StoreLogo);
                LogoImageName = StoreLogo.FileName;
            }
            Store store = new Store()
            {
                Id = Id,
                StoreName = this.StoreName,
                ApplicationUserId = UserId,
                EmailId = this.EmailId,
                PhoneId = this.PhoneId,
                AddressId = this.AddressId,
                StoreStatusId = this.StoreStatusId,
                Email = new Email()
                {
                    Id = this.EmailId,
                    EmailAddress = this.Email
                },
                Phone = new Phone()
                {
                    Id = this.PhoneId,
                    PhoneNumber = this.Phone
                },

                Address = new Address
                {
                    Id = this.AddressId,
                    Division = this.Division,
                    District = this.District,
                    Thana = this.Thana,
                    PostOffice = this.post,
                    RoadNumber = this.RoadNo,


                },
                StoreImages = new List<Image>()
                {
                    new Image()
                    {
                        Url=CoverPhotUrl,
                        Name=CoverImageName,
                        ImageFor="CoverImage"
                    },
                    new Image()
                    {
                        Url=LogoUrl,
                        Name=LogoImageName,
                        ImageFor="LogoImage"
                    }
                }

            };
            if (CoverImageId != null)
                store.StoreImages[0].Id = (int)CoverImageId;
            if (LogoImageId != null)
                store.StoreImages[1].Id = (int)LogoImageId;
            await _storeUnit.UpdateServiceAsync(store);
        }
    }
}
