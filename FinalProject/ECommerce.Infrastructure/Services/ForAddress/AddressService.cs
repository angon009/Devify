using AutoMapper;
using ECommerce.Core.Entities.Stores;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services.ForAddress
{
    public class AddressService : IAddressService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IEcommerceUnitOfWork ecommerceUnitOfWork,
            IMapper mapper, ILogger<AddressService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<Address>> GetAddressAsync(Guid applicationId)
        {
            try
            {
                var addresses = await _ecommerceUnitOfWork.Addresses.GetAsync(x =>
                                                      x.ApplicationId == applicationId, string.Empty);
                return addresses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
