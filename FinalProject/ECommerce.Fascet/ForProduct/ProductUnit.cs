using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForProduct
{
    public class ProductUnit : IProductUnit
    {
        private IProductService _productService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public ProductUnit(IProductService productService,
            IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _productService = productService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        #region Asynchronous Methods
        public async Task CreateServiceAsync(Product product)
        {
            await _productService.CreateProductAsync(product);
            await _ecommerceUnitOfWork.SaveAsync();

        }

        public async Task UpdateServiceAsync(Product product)
        {
            await _productService.UpdateProductAsync(product);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _productService.DeleteProductAsync(id);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        #endregion

        #region Non-Asynchronous Methods
        public void CreateService(Product product)
        {
            _productService.CreateProduct(product);

            _ecommerceUnitOfWork.Save();
        }

        public void UpdateService(Product product)
        {
            _productService.UpdateProduct(product);

            _ecommerceUnitOfWork.Save();
        }

        public void DeleteService(int id)
        {
            _productService.DeleteProduct(id);

            _ecommerceUnitOfWork.Save();
        }

        public Task<(int total, int totalDisplay, IList<Product> records)> GetServiceAsync(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }

        public (int total, int totalDisplay, IList<Product> records) GetService(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateQuantityServiceAsync(int id,int quantity)
        {
            var product = await _productService.GetProductAsync(id);
            product.Quantity = quantity;

            await _productService.ChangeProductQuantityAsync(product);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        // Increase Product Quantity
        public async Task IncreaseQuantityServiceAsync(int id, int quantity)
        {
            var product = await _productService.GetProductAsync(id);
            product.Quantity += quantity;

            await _productService.ChangeProductQuantityAsync(product);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        // Decrease Product Quantity
        public async Task DecreaseQuantityServiceAsync(int id, int quantity)
        {
            var product = await _productService.GetProductAsync(id);
            product.Quantity -= quantity;

            await _productService.ChangeProductQuantityAsync(product);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        #endregion
    }
}
