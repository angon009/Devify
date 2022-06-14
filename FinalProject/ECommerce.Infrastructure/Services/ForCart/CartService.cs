using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using CartEntity = ECommerce.Core.Entities.Orders.Cart;

namespace ECommerce.Infrastructure.Services.ForCart
{
    public class CartService: ICartService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(IEcommerceUnitOfWork ecommerceUnitOfWork,
            IMapper mapper, ILogger<CartService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task CreateOrUpdateCartAsync(Cart cart)
        {
            _logger.LogInformation("You are currently at CreateOrUpdateCartAsync in Cart Service");
            var cartEntity = _mapper.Map<CartEntity>(cart);
            try
            {
                await _ecommerceUnitOfWork.Carts.AddOrModify(cartEntity,
                        x => x.ProductId == cart.ProductId &&
                        x.ApplicationUserId == cart.ApplicationUserId);
                await _ecommerceUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task DeleteCartAsync(Cart cart)
        {
            _logger.LogInformation("You are currently at DeleteCartAsync in Cart Service");
            var cartEntity = _mapper.Map<CartEntity>(cart);
            try
            {
                await _ecommerceUnitOfWork.Carts.RemoveAsync(cartEntity);
                await _ecommerceUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<List<Cart>> GetCartAsync(Guid id)
        {
            try
            {
                var getCarts = await _ecommerceUnitOfWork.Carts.GetAsync(x => 
                x.ApplicationUserId == id, string.Empty);
                var carts = _mapper.Map<List<Cart>>(getCarts);
                return carts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}