using ECommerce.Core.Entities.Orders;
using ECommerce.Data;

namespace ECommerce.Infrastructure.Repositories.ForCart
{
    public interface ICartRepository: IRepository<Cart, Guid>
    {
    }
}
