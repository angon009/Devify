using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class StoreListModel
    {
        private readonly IStoreService _storeService;

        public StoreListModel(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public object GetPagedStores(DataTablesAjaxRequestModel dataTableAjaxRequestModel)
        {
            var data = _storeService.GetStores(
                dataTableAjaxRequestModel.PageIndex,
                dataTableAjaxRequestModel.PageSize,
                dataTableAjaxRequestModel.SearchText,
                dataTableAjaxRequestModel.GetSortText(new string[] { "StoreName", "Email.EmailAddress" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.StoreName,
                            record.Email.EmailAddress,
                            record.StoreStatus.Status,
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}