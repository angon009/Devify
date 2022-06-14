using AutoMapper;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using ProductEntity = ECommerce.Core.Entities.Products.Product;
using ImageEntity = ECommerce.Core.Entities.Common.Image;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public class ProductService : IProductService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<ProductService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region Asynchronus Methods
        public async Task<List<Product>> GetProductsByStoreIdAsync(int StoreId)
        {
            var result =await _ecommerceUnitOfWork.Products.GetAsync(x => x.StoreId == StoreId, string.Empty);
            List<Product> products = new List<Product>();
            foreach (var product in result)
            {
                var entity = _mapper.Map<Product>(product);
                products.Add(entity);
            }
            return products;
        }
        public async Task DiscountAssignToMultiProducts(int[] productsIdlist,int discountId)
        {
            foreach (var item in productsIdlist)
            {
                var entity = await _ecommerceUnitOfWork.Products.GetByIdAsync(item);
                entity.DiscountId = discountId;
            }
            await _ecommerceUnitOfWork.SaveAsync();
        }
        public async Task CreateProductAsync(Product product)
        {
            try
            {
                var productCount = 0;

                if (productCount == 0)
                {
                    var entity = _mapper.Map<ProductEntity>(product);

                    await _ecommerceUnitOfWork.Products.AddAsync(entity);
                }
                else
                {
                    throw new DuplicateDataException("Product with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                var productEntity = await _ecommerceUnitOfWork.Products.GetByIdAsync(product.Id);
                if (product!.ProductImages!.Count > 0)
                {
                    await _ecommerceUnitOfWork.Images.RemoveAsync(x => x.ProductId == product.Id);
                    foreach (var item in product.ProductImages)
                    {
                        ImageEntity entity = _mapper.Map<ImageEntity>(item);
                        await _ecommerceUnitOfWork.Images.AddAsync(entity);
                    }
                }

                await Task.Run(() =>
                {
                    productEntity = _mapper.Map(product, productEntity);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _ecommerceUnitOfWork.Images.RemoveAsync(r => r.ProductId == id);
                await _ecommerceUnitOfWork.Products.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<Product> GetProductAsync(int id)
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
        public async Task<Product> GetStoreProductAsync(int id)
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
        public async Task<IList<Product>> GetStoreProductsAsync(int storeId)
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

        //get product colors
        public async Task<IList<ProductColor>> GetProductColorsAsync(int productId)
        {
            try
            {
                var productcolorEntities = await _ecommerceUnitOfWork.ProductColors.GetAsync(
                        x => x.ProductId == productId, "Color");

                List<ProductColor> productColors = new List<ProductColor>();

                await Task.Run(() =>
                {
                    foreach (var entity in productcolorEntities)
                    {
                        productColors.Add(_mapper.Map<ProductColor>(entity));
                    }
                });
                return productColors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<IList<Product>> GetProductsAsync()
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
        public async Task<(int total, int totalDisplay, IList<Product> records)> GetProductsAsync(
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

        //get categories from storeId
        public async Task<IList<Category>> GetCategoryAsync(int storeId)
        {
            try
            {
                var categoryEntities = await _ecommerceUnitOfWork.Categories.GetAsync(
                        x => x.StoreId == storeId, "SubCategories");
                List<Category> categories = new List<Category>();

                await Task.Run(() =>
                {
                    foreach (var entity in categoryEntities)
                    {
                        categories.Add(_mapper.Map<Category>(entity));
                    }
                });
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        //get brand Names
        public async Task<IList<string?>> GetBrandsAsync(int storeId)
        {
            try
            {
                var productEntities = await _ecommerceUnitOfWork.Products.GetAsync(x
                        => x.StoreId == storeId, "");

                return productEntities.Select(y => y.Brand).Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        //for data table
        public async Task<(int total, int totalDisplay, IList<Product> records)> 
            GetProductsAsync(int storeId, int pageIndex, int pageSize, string searchText, 
            string orderBy)
        {
            List<Product> products = new List<Product>();

            var result = await _ecommerceUnitOfWork.Products.GetDynamicAsync
                (x => x.Name != null && x.Name.Contains(searchText) && x.StoreId== storeId,
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
            GetProductsAsync(int pageIndex, int pageSize, int storeId, int? categoryId, 
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

        #region Non-Asynchronus Methods
        public void CreateProduct(Product product)
        {
            try
            {
                var productCount = _ecommerceUnitOfWork.Products
                        .GetCount(s => s.Name == product.Name);

                if (productCount == 0)
                {
                    var entity = _mapper.Map<ProductEntity>(product);

                    _ecommerceUnitOfWork.Products.Add(entity);
                }
                else
                {
                    throw new DuplicateDataException("Product with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                var productCount = _ecommerceUnitOfWork.Products
                        .GetCount(s => s.Name == product.Name && s.Id == product.Id);

                if (productCount == 0)
                {
                    var productEntity = _ecommerceUnitOfWork.Products.GetById(product.Id);
                    productEntity = _mapper.Map(product, productEntity);
                }
                else
                {
                    throw new DuplicateDataException("Product with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                _ecommerceUnitOfWork.Products.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public Product GetProduct(int id)
        {
            try
            {
                var productEntity = _ecommerceUnitOfWork.Products.GetById(id);
                var product = _mapper.Map<Product>(productEntity);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public IList<Product> GetProducts()
        {
            try
            {
                var productEntities = _ecommerceUnitOfWork.Products.GetAll();

                List<Product> products = new List<Product>();


                foreach (ProductEntity entity in productEntities)
                {
                    products.Add(_mapper.Map<Product>(entity));
                }
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public (int total, int totalDisplay, IList<Product> records) GetProducts(int pageIndex,
            int pageSize, string searchText, string orderBy)
        {
            List<Product> products = new List<Product>();

            var result = _ecommerceUnitOfWork.Products.GetDynamic
                (x => x.Name != null && x.Name.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);

            foreach (ProductEntity entity in result.data)
            {
                products.Add(_mapper.Map<Product>(entity));
            }
            return (result.total, result.totalDisplay, products);
        }

        public async Task ChangeProductQuantityAsync(Product product)
        {
            try
            {
                var productEntity = await _ecommerceUnitOfWork.Products.GetByIdAsync(product.Id);

                await Task.Run(() =>
                {
                    productEntity = _mapper.Map(product, productEntity);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        #endregion
    }
}