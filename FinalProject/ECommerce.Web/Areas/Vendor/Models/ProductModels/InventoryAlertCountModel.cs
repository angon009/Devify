using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Models.ProductModels
{
    public class InventoryAlertCountModel
    {
        public int Id { get; set; }
        public int MinimumStock { get; set; }
        public bool isActive { get; set; }
        public string status { get; set; }
        public int StoreId { get; set; }
        private IInventoryAlertUnit _inventoryAlert;
        private IInventoryAlertService _inventoryService;
        private IAccountRepository _accountRepository;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public InventoryAlertCountModel()
        {

        }
        public InventoryAlertCountModel(IInventoryAlertUnit inventoryAlert,
            IMapper mapper,IInventoryAlertService inventoryService,
            IAccountRepository accountRepo)
        {
            _inventoryAlert = inventoryAlert;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _accountRepository = accountRepo;
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope= scope;
            _inventoryAlert = _scope.Resolve<IInventoryAlertUnit>();
            _inventoryService = _scope.Resolve<IInventoryAlertService>();
            _mapper = _scope.Resolve<IMapper>();
            _accountRepository = _scope.Resolve<IAccountRepository>();
        }
        public async Task CreateOrUpdateInventoryAlert()
        {
            var businessEntity = _mapper.Map<InventoryAlert>(this);
            await _inventoryAlert.CreateOrUpdateServiceAsync(businessEntity);
        }
        public async Task<InventoryAlertCountModel> GetInventorybyStoreId()
        {
            var BusinessEntity = await _inventoryService.GetInventoryAlertByStoreIdAsync(this.StoreId);
            return _mapper.Map<InventoryAlertCountModel>(BusinessEntity);
        }
        internal async Task<object> GetOutOfStockProducts(DataTablesAjaxRequestModel model, int StoreId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _inventoryService.GetInventoryAsync(model.PageIndex,model.PageSize,
                model.GetSortText(new string[] { "Name" }),StoreId,1);

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Name,
                            record.brand,
                            record.ExpireDate.CurrentZone(user.TimeZone!).ToString()!,
                            record.Quantity.ToString(),
                            record.saleprice.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };

        }
        internal async Task<object> GetRunningOutOfStockProducts(DataTablesAjaxRequestModel model, int StoreId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _inventoryService.GetInventoryAlertAsync(model.PageIndex, model.PageSize,
                model.GetSortText(new string[] { "Name" }), StoreId);

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Name,
                            record.brand,
                            record.ExpireDate.CurrentZone(user.TimeZone!).ToString()!,
                            record.Quantity.ToString(),
                            record.saleprice.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };

        }
        internal async Task<(int MinCount,int OutStockcount)> GetMinAndOutStockCount(DataTablesAjaxRequestModel model,int StoreId)
        {
            var OutStock = await _inventoryService.GetInventoryAsync(model.PageIndex, model.PageSize,
                "Name", StoreId, 1);
            var MinCount = await _inventoryService.GetInventoryAlertAsync(model.PageIndex, model.PageSize,
                "Name", StoreId);
            return(MinCount.totalDisplay, OutStock.totalDisplay);
        }
        internal async Task<(int TotalProducts, int TotalOrders, int TotalSales)> GetDashboardValues(int storeId)
        {
            return await _inventoryService.GetDashboardValues(storeId);
        }
    }
}
