using Autofac;
using AutoMapper;
using ECommerce.Membership.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.Profile.Models
{
    public class SettingsModel
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private ILifetimeScope _scope;

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "Image")]
        public string? Image { get; set; }
        public string? Gender { get; set; }
        [Required]
        [Display(Name = "Birthdate")]
        public DateTime Birthdate { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Division")]
        public string? Division { get; set; }
        [Required]
        [Display(Name = "District")]
        public string? District { get; set; }
        [Required]
        [Display(Name = "Thana")]
        public string? Thana { get; set; }
        public string? PostOffice { get; set; }
        [Required]
        [Display(Name = "Road Number")]
        public string? RoadNumber { get; set; }
        [Required]
        [Display(Name = "Time Zone")]
        public string? TimeZoneId { get; set; }
        public List<SelectListItem> TimeZones { get; set; }
        public IFormFile UploadImage { get; set; }
        public SettingsModel()
        {
        }
        public SettingsModel(IAccountRepository accountRepository, IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _accountRepository = _scope.Resolve<IAccountRepository>();
            _webHostEnvironment = _scope.Resolve<IWebHostEnvironment>();
        }
        public void GetTimeZones()
        {
            TimeZones = new List<SelectListItem>();
            foreach (var zone in TimeZoneInfo.GetSystemTimeZones())
            {
                TimeZones.Add(new SelectListItem
                {
                    Text = zone.DisplayName,
                    Value = zone.Id
                });
            }

        }
        public async Task GetCurrentUserAync()
        {
            var appUser = await _accountRepository.GetCurrentUserAsync();
            FirstName = appUser.FirstName;
            LastName = appUser.LastName;
            Birthdate = appUser.Birthdate;
            Email = appUser.Email;
            PhoneNumber = appUser.PhoneNumber;
            Image = appUser.Image;
            Gender = appUser.Gender;
            TimeZoneId = appUser.TimeZone;
            var address = appUser.Address!.FirstOrDefault();
            if (address != null)
            {
                Division = address!.Division;
                District = address.District;
                Thana = address.Thana;
                PostOffice = address.PostOffice;
                RoadNumber = address.RoadNumber;
            }
        }
        public async Task<bool> UpdateUser()
        {

            var user = await _accountRepository.GetCurrentUserAsync();

            if (UploadImage != null)
            {
                var folder = "Profile/images/people/";
                var _image = string.Format("{0}{1}", Guid.NewGuid().ToString(), UploadImage.FileName);
                folder += _image;
                var serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await UploadImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                user.Image = _image;
            }
            else
            {
                user.Image = Image;
            }
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Birthdate = Birthdate;
            user.PhoneNumber = PhoneNumber;
            user.Email = Email;
            user.Gender = Gender;
            user.TimeZone = TimeZoneId;
            var address = user.Address!.Find(x => x.ApplicationId == user.Id);
            if (address != null)
            {
                address.Division = Division;
                address.District = District;
                address.Thana = Thana;
                address.PostOffice = PostOffice;
                address.RoadNumber = RoadNumber;
            }
            else
            {
                user.Address = new List<Core.Entities.Stores.Address>
                {
                    new Core.Entities.Stores.Address
                    {
                        ApplicationUser = user,
                        Division = Division,
                        District = District,
                        Thana = Thana,
                        PostOffice = PostOffice,
                        RoadNumber = RoadNumber
                    }
                };
            }
            return await _accountRepository.UpdateAccountAsync(user);

        }
    }
}
