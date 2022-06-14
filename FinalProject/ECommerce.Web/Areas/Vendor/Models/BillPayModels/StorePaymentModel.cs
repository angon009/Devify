using ECommerce.Infrastructure.Services.ForStorePayments;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.Vendor.Models.BillPayModels
{
    public class StorePaymentModel
    {
        public string ConfirmUrl { get; set; } = "/Vendor/Payment/PaymentConfirmed";
        public string FailedUrl { get; set; } = "/Vendor/Payment/PaymentFailed";
        public string CencelUrl { get; set; } = "/Vendor/Payment/PaymentCenceled";

        private IStorePaymentService _storePaymentService;
        private readonly IAccountRepository _accountRepository;

        public StorePaymentModel(IStorePaymentService storePaymentService, 
            IAccountRepository accountRepository)
        {
            _storePaymentService = storePaymentService;
            _accountRepository = accountRepository;
        }
        public StorePaymentModel()
        {

        }

        internal async Task<object> GetPagedPayments(DataTablesAjaxRequestModel model, int storeId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data = await _storePaymentService.GetStorePaymentsAsync(
                storeId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "PaymentDate", "PaymentAmount", "TransactionId" }));
            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.TransactionId,
                            record.PaymentDate.CurrentZone(user.TimeZone!).ToString("dd-MM-yyyy HH:mm"),
                            record.PaymentAmount.ToString()
                        }).ToArray()
            };
        }
    }
}
