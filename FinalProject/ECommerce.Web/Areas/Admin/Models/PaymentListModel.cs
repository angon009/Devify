using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Infrastructure.Services.ForStorePayments;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class PaymentListModel
    {
        private readonly IStorePaymentService _paymentService;

        public PaymentListModel(IStorePaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<object> GetPagedStoresPayment(DataTablesAjaxRequestModel dataTableAjaxRequestModel)
        {
            var data = await _paymentService.GetStorePaymentsAsync(
                dataTableAjaxRequestModel.PageIndex,
                dataTableAjaxRequestModel.PageSize,
                dataTableAjaxRequestModel.SearchText,
                dataTableAjaxRequestModel.GetSortText(new string[] { "Store.StoreName" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.TransactionId,
                            record.Store.StoreName,
                            record.PaymentAmount.ToString(),
                            record.PaymentDate.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}