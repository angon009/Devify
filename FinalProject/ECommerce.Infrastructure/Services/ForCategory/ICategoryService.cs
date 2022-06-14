using ECommerce.Infrastructure.BusinessObjects.Products;

namespace ECommerce.Infrastructure.Services.ForCategory
{
    public interface ICategoryService
    {
        #region Asynchronus Methods
        Task CreateCategoryAsync(Category category); // For Create
        Task UpdateCategoryAsync(Category category); // For Update
        Task DeleteCategoryAsync(int id); // For Delete
        Task<Category> GetCategoryAsync(int id); // For Read
        Task<IList<Category>> GetCategoriesAsync(int StoreId); // For Read
        Task<(int total, int totalDisplay, IList<Category> records)> GetCategoriesAsync(int storeId, int pageIndex,
            int pageSize, string searchText, string orderBy); //For Read
        #endregion



        #region Non-Asynchronus Methods
        void CreateCategory(Category category); // For Create
        void UpdateCategory(Category category); // For Update
        void DeleteCategory(int id); // For Delete
        Category GetCategory(int id); // For Read
        IList<Category> GetCategories(); // For Read
        (int total, int totalDisplay, IList<Category> records) GetCategories(int pageIndex, int pageSize,
            string searchText, string orderBy); //For Read
        #endregion
    }
}
