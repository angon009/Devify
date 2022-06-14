using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForCart;
using ECommerce.Infrastructure.Services.ForOrder;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Membership.Repositories;

namespace ECommerce.Web.Models
{
    public class ShoppingCartModel
    {
        private IMapper _mapper;
        private ILifetimeScope _Scope;
        private IAccountRepository _accountRepo;
        private IProductService _productService;
        private IOrderService _orderService;
        private IProductUnit _productUnit;
        public List<CartItemModel> _cartItemsModel;
        public ICartService _CartService;
        public string ImageUrl { get { return GetImage.Url; } }

        public double TotalAmount { get; set; }
        public double DiscountTotal { get; set; }
        public double TotalCostAmount { get; set; }
        public int? StoreId { get; set; }
        public ShoppingCartModel()
        {

        }
        public ShoppingCartModel(ICartService cartService, IMapper mapper,
            IAccountRepository accountRepo, IProductService productService,
            IOrderService orderService, IProductUnit productUnit)
        {
            _CartService = cartService;
            _mapper = mapper;
            _accountRepo = accountRepo;
            _productService = productService;
            _orderService = orderService;
            _productUnit = productUnit;
            _cartItemsModel = new List<CartItemModel>();
        }
        public void Resolve(ILifetimeScope scope)
        {
            _Scope = scope;
            _CartService = scope.Resolve<ICartService>();
            _mapper = scope.Resolve<IMapper>();
            _accountRepo = scope.Resolve<IAccountRepository>();
            _productService = scope.Resolve<IProductService>();
            _orderService = scope.Resolve<IOrderService>();
            _productUnit = scope.Resolve<IProductUnit>();
        }
        public async Task CreateOrUpdateAsync(List<CartItemModel> cartItems)
        {
            foreach (var cart in cartItems)
            {
                cart.ApplicationUserId = _accountRepo.GetCurrentUserAsync().Result.Id;
                var cartObj = _mapper.Map<Cart>(cart);
                if (cart.Quantity <= 0)
                {
                    await _CartService.DeleteCartAsync(cartObj);
                }
                else
                    await _CartService.CreateOrUpdateCartAsync(cartObj);
            }

        }
        internal async Task GetProductsAsync(List<CartItemModel> cartItemModels)
        {
            foreach (var cartItem in cartItemModels)
            {
                var product = await _productService.GetStoreProductAsync(cartItem.ProductId);
                var cartProduct = new CartItemModel
                {
                    Id = cartItem.Id,
                    Product = product,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    TotalPrice = product.SalePrice * cartItem.Quantity,
                    TotalCostPrice = product.CostPrice * cartItem.Quantity,
                    DiscountId = product.DiscountId != null ? product.DiscountId : null,
                    DiscountTotal = product.DiscountId != null ? ((product.SalePrice / 100 * product.Discount!.Percentage) * cartItem.Quantity):0,
                    ApplicationUserId = cartItem.ApplicationUserId
                };

                if(StoreId == null)
                    StoreId = product.StoreId;
                _cartItemsModel.Add(cartProduct);
            }
        }
        internal async Task OrderPlaceAsync(List<CartItemModel> cartItemModels)
        {
            var orders = new List<Order>();
            var orderDetails = new List<OrderDetail>();
            foreach (var cartItem in cartItemModels)
            {
                orderDetails.Add(new OrderDetail
                {
                    DiscountId = (cartItem.DiscountId != 0) ? (int)cartItem.DiscountId! : 0,
                    Quantity = cartItem.Quantity,
                    ProductId = cartItem.ProductId,
                });

                // Decrease Product
                await _productUnit.DecreaseQuantityServiceAsync(cartItem.ProductId, cartItem.Quantity);
            }
            orders.Add(new Order
            {
                ApplicationUserId = Guid.Parse(_accountRepo.GetUserId()),
                OrderDate = DateTime.Now,
                TotalAmount = this.TotalAmount,
                TotalCostAmount = this.TotalCostAmount,
                DiscountTotal = this.DiscountTotal,
                OrderStatusId = 1,
                StoreId = StoreId,
                OrderDetails = orderDetails,
            });
            await _orderService.OrderAsync(orders);

        }

        // all cart items total price
        public double TotalPrice()
        {

            double _total = 0;
            _cartItemsModel.ForEach(cartItem =>
            {
                _total += cartItem.TotalPrice;
            });
            return _total;
        }
        public double TotalCostPrice()
        {

            double _total = 0;
            _cartItemsModel.ForEach(cartItem =>
            {
                _total += cartItem.TotalCostPrice;
            });
            return _total;
        }
        // all cart items total discount
        public double TotalDiscount()
        {

            double? _discount = 0;
            _cartItemsModel.ForEach(cartItem =>
            {
                _discount += cartItem.DiscountTotal;
            });
            return (double)_discount;
        }

        // login user cart items
        public async Task<List<CartItemModel>> GetCartAsync(Guid id)
        {
            var cartItemList = new List<CartItemModel>();
            var cartList = await _CartService.GetCartAsync(id);

            cartList.ForEach(x =>
            {
                cartItemList.Add(new CartItemModel
                {
                    Id = x.Id,
                    ApplicationUserId = x.ApplicationUserId,
                    ProductId = (int)x.ProductId,
                    Quantity = (int)x.Quantity,
                    DiscountId = x.DiscountId
                });
            });
            return cartItemList;
        }

    }
}
