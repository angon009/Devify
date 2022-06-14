using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForCategory;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForCategory
{
    public class CategoryUnit : ICategoryUnit
    {
        private ICategoryService _categoryService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public CategoryUnit(ICategoryService categoryService,
            IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _categoryService = categoryService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        #region Asynchronous Methods
        public async Task CreateServiceAsync(Category category)
        {

            await _categoryService.CreateCategoryAsync(category);

            await _ecommerceUnitOfWork.SaveAsync();

        }

        public async Task UpdateServiceAsync(Category category)
        {
            await _categoryService.UpdateCategoryAsync(category);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async Task<(int total, int totalDisplay, IList<Category> records)> GetServiceAsync(int storeId, int pageIndex, int pageSize, string searchText, string orderBy)
        {
            return await _categoryService.GetCategoriesAsync(storeId, pageIndex, pageSize, searchText, orderBy);
        }
        #endregion


        #region Non-Asynchronus Methods
        public void CreateService(Category category)
        {
            _categoryService.CreateCategory(category);

            _ecommerceUnitOfWork.Save();
        }

        public void UpdateService(Category category)
        {
            _categoryService.UpdateCategory(category);

            _ecommerceUnitOfWork.Save();
        }

        public void DeleteService(int id)
        {
            _categoryService.DeleteCategory(id);

            _ecommerceUnitOfWork.Save();
        }

        public (int total, int totalDisplay, IList<Category> records) GetService(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
