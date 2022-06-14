using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using ProductEntity = ECommerce.Core.Entities.Products.Product;
using ECommerce.Core.StoredProcedureEntites;

namespace ECommerce.Infrastructure.Services.ForStock
{
    public class StockService : IStockService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StockService> _logger;

        public StockService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<StockService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region Asynchronus Methods
        public async Task<Product> GetStockAsync(int id)
        {
            try
            {
                var productEntity = await _ecommerceUnitOfWork.Products.GetByIncludeAsync(x
                        => x.Id == id, "ProductImages,SubCategory");

                Product product = new Product();

                await Task.Run(() =>
                {
                    product = _mapper.Map<Product>(productEntity);
                });

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        //Get Products with including objects
        public async Task<Product> GetStoreStockAsync(int id)
        {
            try
            {
                var productEntity = await _ecommerceUnitOfWork.Products.GetAsync(
                        x => x.Id == id, "Colors,ProductImages,SubCategory,Discount");

                Product product = new Product();

                product = _mapper.Map<Product>(productEntity.FirstOrDefault());

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        //Get Products by store id
        public async Task<IList<Product>> GetStoreStocksAsync(int storeId)
        {
            try
            {
                var productEntities = await _ecommerceUnitOfWork.Products.GetAsync(x
                        => x.StoreId == storeId, "Colors,ProductImages,SubCategory,Discount");

                List<Product> products = new List<Product>();

                await Task.Run(() =>
                {
                    foreach (ProductEntity entity in productEntities)
                    {
                        products.Add(_mapper.Map<Product>(entity));
                    }
                });

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        } 
        public async Task<IList<Product>> GetStocksAsync()
        {
            try
            {
                var productEntities = await _ecommerceUnitOfWork.Products.GetAllAsync();

                List<Product> products = new List<Product>();

                await Task.Run(() =>
                {
                    foreach (ProductEntity entity in productEntities)
                    {
                        products.Add(_mapper.Map<Product>(entity));
                    }
                });

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(int total, int totalDisplay, IList<Product> records)> GetStocksAsync(
            int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<Product> products = new List<Product>();

            var result = await _ecommerceUnitOfWork.Products.GetDynamicAsync
                (x => x.Name != null && x.Name.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (ProductEntity entity in result.data)
                {
                    products.Add(_mapper.Map<Product>(entity));
                }
            });
            return (result.total, result.totalDisplay, products);
        } 
        //for data table
        public async Task<(int total, int totalDisplay, IList<Product> records)> GetStocksAsync(
           int storeId, int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<Product> products = new List<Product>();

            var result = await _ecommerceUnitOfWork.Products.GetDynamicAsync
                (x => x.Name != null && x.Name.Contains(searchText) && x.StoreId == storeId,
                orderBy, string.Empty, pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (ProductEntity entity in result.data)
                {
                    products.Add(_mapper.Map<Product>(entity));
                }
            });
            return (result.total, result.totalDisplay, products);
        }

        //stored Procedure
        public async Task<(int total, int totalDisplay, IList<FilteredProducts> records)> 
            GetStocksAsync(int pageIndex, int pageSize, int storeId, int? categoryId, 
            int? subCategoryId, string brands, int? minimum, int? maximum, string? orderBy)
        {
            //string? xbrands = null;
            //if (brands != null)
            //{
            //    xbrands = string.Join(",", brands);
            //}

            var result = await _ecommerceUnitOfWork.Products.GetProductsAsync(
                pageIndex,
                pageSize,
                orderBy,
                storeId,
                categoryId,
                subCategoryId,
                brands,
                minimum,
                maximum);
            List<FilteredProducts> products = new List<FilteredProducts>();
            foreach (var product in result.data)
            {
                products.Add(product);
            }

            return (result.total, result.totalFiltered, products);
        }
        #endregion
    }
}