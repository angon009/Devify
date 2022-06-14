using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForStore;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.StoreAdmin.Models.StoreModels
{
    public class StoreCreateModel
    {
        [Required]
        [MaxLength(105)]
        [Display(Name ="Store Name")]
        public string StoreName { get; set; }
        [Required]
        public string Email { get; set; }

        public string? SubDomain { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Division { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Thana { get; set; }
        [Display(Name = "Post Office")]
        public string PostOffice { get; set; }
        [Display(Name = "Road Number")]
        public string RoadNumber { get; set; }
        public int Statusid { get; set; } = (int)Status.Active;
        public Guid ApplicationUserId { get; set; }


        private IStoreUnit _storeUnit;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public StoreCreateModel()
        {

        }

        public StoreCreateModel(IMapper mapper, IStoreUnit storeUnit,ILifetimeScope scope)
        {
            _storeUnit = storeUnit;
            _mapper = mapper;
            _scope = scope;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _storeUnit = _scope.Resolve<IStoreUnit>();
            _mapper = _scope.Resolve<IMapper>();
        }

        internal async Task CreateStore()
        {
            var store = new Store()
            {
                StoreName = StoreName,
                SubDomain = SubDomain,
                Phone = new Phone()
                {
                    PhoneNumber = Phone
                },
                Email = new Email()
                {
                    EmailAddress = Email
                },
                Address = new Address()
                {
                    Division = Division,
                    District = District,
                    Thana = Thana,
                    PostOffice = PostOffice,
                    RoadNumber = RoadNumber
                },
                StoreStatusId = Statusid,
                ApplicationUserId = ApplicationUserId
            };
            await _storeUnit.CreateServiceAsync(store);
        }
    }
}
