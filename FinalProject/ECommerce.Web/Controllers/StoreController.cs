using Autofac;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    [ServiceFilter(typeof(StoreSubDomainChecker))]
    public class StoreController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly IAccountRepository _accountRepo;
        private readonly ILogger<StoreController> _logger;

        public StoreController(ILifetimeScope scope, IAccountRepository accountRepo, ILogger<StoreController> logger)
        {
            _scope = scope;
            _accountRepo = accountRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = _scope.Resolve<StoreModel>();
            model.BannerUrl = "Theme/images/banners/banner7.jpg";

            var storeId = await model.GetStoreIdBySubDomain(); //storeId will get dynamically
            if (storeId == 0 || storeId == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await model.GetCategories(storeId);
            ViewBag.Products = await model.GetProducts(storeId);
            return View(model);
        }

        public async Task<IActionResult> Products(ProductListModel search, int pn)
        {
            var model = _scope.Resolve<ProductListModel>();
            var storeModel = _scope.Resolve<StoreModel>();

            var storeId = await storeModel.GetStoreIdBySubDomain(); //storeId will get dynamically
            if (storeId == 0)
            {
                return NotFound();
            }

            model.StoreId = storeId; //storeId will get dynamically
            model.CategoryId = search.CategoryId == null ? model.CategoryId : search.CategoryId;
            model.SubCategoryId = search.SubCategoryId == null ? model.SubCategoryId : 
                search.SubCategoryId;
            model.Brand = search.Brand == null ? model.Brand : search.Brand;
            model.MinimumPrice = search.MinimumPrice == null ? model.MinimumPrice : 
                search.MinimumPrice;
            model.MaximumPrice = search.MaximumPrice == null ? model.MaximumPrice : 
                search.MaximumPrice;
            model.PageIndex = pn == 0 ? 1 : pn;
            
            
            var getProducts = model.GetFilteredProducts();
            ViewBag.Products = getProducts;

            long total = (long)ViewBag.Products.Result.Item2;
            ViewBag.Pagination = PagingModel.SetPaging(model.PageIndex, 3, total, 
                "activeLink", Url.Action("Products", "Store", model), "disableLink");
            ViewBag.Categories = await model.GetCategories(model.StoreId);
            ViewBag.Brands = await model.GetBrands(model.StoreId);
            return View(model);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var model = _scope.Resolve<ProductDetailsModel>();
            try
            {
                ViewBag.Product = await model.GetProductAsync(id);
                ViewBag.ProductColors = await model.GetColorsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }
        public async Task<IActionResult> Cart()
        {
            var model = _scope.Resolve<ShoppingCartModel>();
            var cartSession = new List<CartItemModel>();

            try
            {
                if (TempData.Peek<IList<CartItemModel>>("CartSession") != null &&
                        TempData.Peek<IList<CartItemModel>>("CartSession").Count > 0)
                {
                    cartSession = TempData.Peek<IList<CartItemModel>>("CartSession").ToList();

                }
                await model.GetProductsAsync(cartSession);

                // Now save it into database
                if (_accountRepo.IsAuthenticated()) // is auth user?
                    await model.CreateOrUpdateAsync(cartSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(model);
        }
        public async Task<IActionResult> Order(ShoppingCartModel model)
        {
            try
            {
                if (_accountRepo.IsAuthenticated())
                {
                    var cartSession = GetCartSession();
                    model.Resolve(_scope);
                    await model.OrderPlaceAsync(cartSession); // if order failed to place then reasign session

                    cartSession.ForEach(x => x.Quantity = 0);
                    await model.CreateOrUpdateAsync(cartSession);
                    TempData.Peek<IList<CartItemModel>>("CartSession");
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Login", "Account");
        }
        public List<CartItemModel> GetCartSession()
        {
            var cartItemList = new List<CartItemModel>();

            try
            {
                if (TempData.Peek<IList<CartItemModel>>("CartSession") != null &&
                        TempData.Peek<IList<CartItemModel>>("CartSession").Count > 0)
                {
                    cartItemList = TempData.Get<IList<CartItemModel>>("CartSession").ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return cartItemList;
        }
        public async Task<IActionResult> EditCart(int id, int qty)
        {
            var model = _scope.Resolve<ShoppingCartModel>();
            var cartSession = GetCartSession();

            var existItem = cartSession.FirstOrDefault(x => x.ProductId == id);
            try
            {
                if (existItem != null)
                {
                    existItem.Quantity = qty;
                }
                else
                {
                    cartSession.Add(new CartItemModel
                    {
                        Id = Guid.NewGuid(),
                        //Product = await model.GetProductAsync(id),
                        ProductId = id,
                        Quantity = qty,
                    });
                }

                // Now save it into database
                if (_accountRepo.IsAuthenticated()) // is auth user?
                    await model.CreateOrUpdateAsync(cartSession);

                if (qty <= 0)
                    cartSession.Remove(existItem);

                TempData.Put("CartSession", cartSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return RedirectToAction(nameof(Cart));
        }
        #region Ajax
        // Ajax call back
        public async Task<JsonResult> AddtoCart(int id, int qty = 1, int discountId = 0)
        {
            var model = _scope.Resolve<ShoppingCartModel>();
            var cartSession = GetCartSession();

            var existItem = cartSession.FirstOrDefault(x => x.ProductId == id);
            if (existItem != null)
            {
                existItem.Quantity += qty;
            }
            else
            {
                cartSession.Add(new CartItemModel
                {
                    Id = Guid.NewGuid(),
                    DiscountId = discountId,
                    //Product = await model.GetProductAsync(id),
                    ProductId = id,
                    Quantity = qty,
                });
            }

            TempData.Put("CartSession", cartSession);

            // Now save it into database
            try
            {
                if (_accountRepo.IsAuthenticated()) // is auth user?
                    await model.CreateOrUpdateAsync(cartSession);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return Json(true);
        }

        // Count method for ajax call
        public int CountCart()
        {
            var count = 0;
            if (TempData.Peek<IList<CartItemModel>>("CartSession") != null)
            {
                count = TempData.Peek<IList<CartItemModel>>("CartSession").Count;

            }
            return count;
        } 
        #endregion
    }
}
