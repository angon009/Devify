using ECommerce.Infrastructure.BusinessObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public interface IDiscountService
    {
        Task CreateDiscountAsync(Discount discount);
        Task UpdateDiscountAsync(Discount discount);
        Task<Discount> GetDiscountAsync(int id);
        Task DeleteDiscountAsync(int Id);
        Task<IList<Discount>> GetDiscountsAsync(int StoreId);
        Task<(int total, int totalDisplay, IList<Discount> records)> GetDiscountsAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy);
    }
}
