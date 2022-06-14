using Autofac;
using AutoMapper;
using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Membership.Repositories;
using ECommerce.Membership.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.StoreAdmin.Models.StoreModels
{
    public class StoreListViewModel
    {
        private IStoreService _storeService;
        private ILifetimeScope _scope;
        private IMapper _mapper;
        private IAccountRepository _accountRepo;
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public string StatusId { get; set; }
        public string Address { get; set; }
        public Image StoreImage { get; set; }
        public StoreListViewModel()
        {

        }

        public StoreListViewModel(IMapper mapper, IStoreService storeService, IAccountRepository accountRepo)
        {

            _mapper = mapper;
            _storeService = storeService;
            _accountRepo = accountRepo;
        }



        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _storeService = _scope.Resolve<IStoreService>();
            _accountRepo = _scope.Resolve<IAccountRepository>();
            _mapper = _scope.Resolve<IMapper>();
        }
        public async Task<object> GetPagedStores(DataTablesAjaxRequestModel model)
        {
            ApplicationUser = await _accountRepo!.GetCurrentUserAsync();
            Guid UserId = (ApplicationUser.Id);
            var data = await _storeService.GetStoresByUserIdAsync(
                UserId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "StoreName", "Email", "Phone", "Address", "StoreStatus" }));

            var result = new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.StoreName!,
                            record.Email!.EmailAddress!,
                            record.Phone!.PhoneNumber!,
                            record.Address!.Division+","
                            +record.Address.District+","+record.Address.Thana,
                            record.StoreStatus!.Status!,
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
            return result;
        }

        public async Task<IList<StoreListShowModel>> GetStoresByUserId(Guid Id)
        {

            var data1 = await _storeService.GetStoreByUserIdAsync(Id);
            var data = data1.stores;
            var viewModelData = new List<StoreListShowModel>();
            if (data is not null)
            {
                foreach (var store in data)
                {
                    StoreListShowModel model = new StoreListShowModel()
                    {
                        Id = store.Id,
                        StoreName = store.StoreName!,
                        Email = store.Email!.EmailAddress!,
                        Phone = store.Phone!.PhoneNumber!,
                        Address = $"{store.Address!.Division}, {store.Address.District},{store.Address.Thana},{store.Address.RoadNumber}",
                        StatusId = store.StoreStatusId!.ToString(), 
                        ApplicationUserId = (Guid)store.ApplicationUserId
                    };
                    viewModelData.Add(model);
                }
            }

            return viewModelData;
        }



    }
}
