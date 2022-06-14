using Autofac;
using ECommerce.Fascet.ForStore;

namespace ECommerce.Web.Areas.Admin.Models
{

    public class StoreStatusModel
    {
        private IStoreUnit _storeUnit;
        private ILifetimeScope _scope;

        public string? StoreName { get; set; }
        public int? EmailId { get; set; }
        public int? PhoneId { get; set; }
        public int? AddressId { get; set; }
        public int? StoreStatusId { get; set; }

        public StoreStatusModel()
        {

        }
        public StoreStatusModel(IStoreUnit storeUnit)
        {
            _storeUnit = storeUnit;
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _storeUnit = _scope.Resolve<IStoreUnit>();
        }
        public async Task EnableStoreAsync(int id)
        {
            await _storeUnit.StoreEnableServiceAsync(id);
        }
        public async Task DisableStoreAsync(int id)
        {
            await _storeUnit.StoreDisableServiceAsync(id);
        }
        public async Task DeleteStoreAsync(int id)
        {
            await _storeUnit.DeleteServiceAsync(id);
        }

    }

}