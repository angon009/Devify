using ECommerce.Core.Entities.Stores;

namespace ECommerce.Infrastructure.Services.ForAddress
{
    public interface IAddressService
    {
        Task<IList<Address>> GetAddressAsync(Guid applicationId);
    }
}
