using ECommerce.Infrastructure.BusinessObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ForCart
{
    public interface ICartService
    {
        Task CreateOrUpdateCartAsync(Cart cart);
        Task DeleteCartAsync(Cart cart);
        Task<List<Cart>> GetCartAsync(Guid id);
    }
}
