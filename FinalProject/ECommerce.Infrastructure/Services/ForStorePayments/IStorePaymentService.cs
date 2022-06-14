using ECommerce.Infrastructure.BusinessObjects.Common;

namespace ECommerce.Infrastructure.Services.ForStorePayments
{
    public interface IStorePaymentService
    {
        Task CreatePaymentAsync(StorePayments storePayments);
        Task<(int total, int totalDisplay, IList<StorePayments> records)> GetStorePaymentsAsync(int storeId,
            int pageIndex, int pageSize, string searchText, string orderBy);//Get Store Specified

        Task<(int total, int totalDisplay, IList<StorePayments> records)> GetStorePaymentsAsync(
            int pageIndex, int pageSize, string searchText, string orderBy); //Get All
    }
}
