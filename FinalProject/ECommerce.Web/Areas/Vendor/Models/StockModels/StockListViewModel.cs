using ECommerce.Fascet.ForStock;
using ECommerce.Infrastructure.Services.ForStock;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Models.StockModels
{
    public class StockListViewModel
    {
        private IStockService _stockService;
        private IStockUnit _stockUnit;

        public StockListViewModel()
        {

        }
        public StockListViewModel(IStockService stockService , IStockUnit stockUnit)
        {
            _stockService = stockService;
            _stockUnit = stockUnit;
        } 
        public async Task<object> GetPagedStores(DataTablesAjaxRequestModel model, int StoreId)
        {
            var data = await _stockService.GetStocksAsync(
                StoreId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "Name", "Brand","Quantity"}));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name,
                                record!.Brand!, 
                                record.Quantity.ToString(),  
                                record.Id.ToString() 
                        }
                    ).ToArray()
            };
        }
    }
}
