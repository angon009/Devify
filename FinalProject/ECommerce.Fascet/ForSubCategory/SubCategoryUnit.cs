using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForSubCategory;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Fascet.ForSubCategory
{
    public class SubCategoryUnit : ISubCategoryUnit
    {
        private ISubCategoryService _subCategoryService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public SubCategoryUnit(ISubCategoryService subCategoryService,
            IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _subCategoryService = subCategoryService;
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }
        #region Asynchronous Methods
        public async Task CreateServiceAsync(SubCategory subCategory)
        {

            await _subCategoryService.CreateSubCategoryAsync(subCategory);

            await _ecommerceUnitOfWork.SaveAsync();

        }

        public async Task UpdateServiceAsync(SubCategory subCategory)
        {
            await _subCategoryService.UpdateSubCategoryAsync(subCategory);

            await _ecommerceUnitOfWork.SaveAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _subCategoryService.DeleteSubCategoryAsync(id);

            await _ecommerceUnitOfWork.SaveAsync();
        }
        #endregion



        #region Non-Asynchronous Methods
        public void CreateService(SubCategory subCategory)
        {
            _subCategoryService.CreateSubCategory(subCategory);

            _ecommerceUnitOfWork.Save();
        }

        public void UpdateService(SubCategory subCategory)
        {
            _subCategoryService.UpdateSubCategory(subCategory);

            _ecommerceUnitOfWork.Save();
        }

        public void DeleteService(int id)
        {
            _subCategoryService.DeleteSubCategory(id);

            _ecommerceUnitOfWork.Save();
        }

        public Task<(int total, int totalDisplay, IList<SubCategory> records)> GetServiceAsync(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }

        public (int total, int totalDisplay, IList<SubCategory> records) GetService(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}

