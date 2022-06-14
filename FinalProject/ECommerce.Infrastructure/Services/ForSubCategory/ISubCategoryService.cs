using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.Services.ForSubCategory
{
    public interface ISubCategoryService
    {
        #region Asynchronus Methods
        Task CreateSubCategoryAsync(SubCategory subCategory); // For Create
        Task UpdateSubCategoryAsync(SubCategory subCategory); // For Update
        Task DeleteSubCategoryAsync(int id); // For Delete
        Task<SubCategory> GetSubCategoryAsync(int id); // For Read
        Task<IList<SubCategory>> GetSubCategoriesAsync(int CategoryId); // For Read
        Task<(int total, int totalDisplay, IList<SubCategory> records)> GetSubCategoriesAsync(int categoryId,
            int pageIndex, int pageSize, string searchText, string orderBy); //For Read
        #endregion



        #region Non-Asynchronus Methods
        void CreateSubCategory(SubCategory subCategory); // For Create
        void UpdateSubCategory(SubCategory subCategory); // For Update
        void DeleteSubCategory(int id); // For Delete
        SubCategory GetSubCategory(int id); // For Read
        IList<SubCategory> GetSubCategories(int CategoryId); // For Read
        (int total, int totalDisplay, IList<SubCategory> records) GetSubCategories(int pageIndex, int pageSize,
            string searchText, string orderBy); //For Read
        #endregion
    }
}
