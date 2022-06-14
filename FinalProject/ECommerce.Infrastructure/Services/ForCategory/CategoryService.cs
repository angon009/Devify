using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using CategoryEntity = ECommerce.Core.Entities.Products.Category;

namespace ECommerce.Infrastructure.Services.ForCategory
{
    public class CategoryService : ICategoryService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region Asynchronus Methods
        public async Task CreateCategoryAsync(Category category)
        {
            try
            {
                var categoryCount = await _ecommerceUnitOfWork.Categories
                        .GetCountAsync(s => s.CategoryName == category.CategoryName);

                if (categoryCount == 0)
                {
                    var entity = _mapper.Map<CategoryEntity>(category);

                    await _ecommerceUnitOfWork.Categories.AddAsync(entity);
                }
                else
                {
                    throw new DuplicateDataException("Category with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                var categoryCount = await _ecommerceUnitOfWork.Categories
                        .GetCountAsync(s => s.CategoryName == category.CategoryName && s.Id != category.Id);

                if (categoryCount == 0)
                {
                    var categoryEntity = await _ecommerceUnitOfWork.Categories.GetByIdAsync(category.Id);

                    await Task.Run(() =>
                    {
                        categoryEntity = _mapper.Map(category, categoryEntity);
                    });
                }
                else
                {
                    throw new DuplicateDataException("Category with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                await _ecommerceUnitOfWork.Categories.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            try
            {
                var categoryEntity = await _ecommerceUnitOfWork.Categories.GetByIdAsync(id);

                Category category = new Category();

                await Task.Run(() =>
                {
                    category = _mapper.Map<Category>(categoryEntity);
                });

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<IList<Category>> GetCategoriesAsync(int StoreId)
        {
            try
            {
                var categoryEntities = await _ecommerceUnitOfWork.Categories.GetAsync(x =>
                    x.StoreId == StoreId, string.Empty);

                List<Category> subCategories = new List<Category>();

                await Task.Run(() =>
                {
                    foreach (CategoryEntity entity in categoryEntities)
                    {
                        subCategories.Add(_mapper.Map<Category>(entity));
                    }
                });
                return subCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }
        public async Task<(int total, int totalDisplay, IList<Category> records)> 
            GetCategoriesAsync(int storeId, int pageIndex, int pageSize, 
            string searchText, string orderBy)
        {
            try
            {
                List<Category> subCategories = new List<Category>();

                var result = await _ecommerceUnitOfWork.Categories.GetDynamicAsync
                    (x => x.CategoryName != null && x.StoreId == storeId && x.CategoryName.Contains(searchText),
                    orderBy, string.Empty, pageIndex, pageSize, true);

                await Task.Run(() =>
                {
                    foreach (CategoryEntity entity in result.data)
                    {
                        subCategories.Add(_mapper.Map<Category>(entity));
                    }
                });
                return (result.total, result.totalDisplay, subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        #endregion

        #region Non-Asynchronus Methods
        public void CreateCategory(Category category)
        {
            try
            {
                var categoryCount = _ecommerceUnitOfWork.Categories
                        .GetCount(s => s.CategoryName == category.CategoryName);

                if (categoryCount == 0)
                {
                    var entity = _mapper.Map<CategoryEntity>(category);

                    _ecommerceUnitOfWork.Categories.Add(entity);
                }
                else
                {
                    throw new DuplicateDataException("Category with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                var categoryCount = _ecommerceUnitOfWork.Categories
                        .GetCount(s => s.CategoryName == category.CategoryName && s.Id == category.Id);

                if (categoryCount == 0)
                {
                    var categoryEntity = _ecommerceUnitOfWork.Categories.GetById(category.Id);
                    categoryEntity = _mapper.Map(category, categoryEntity);
                }
                else
                {
                    throw new DuplicateDataException("Category with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                _ecommerceUnitOfWork.Categories.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public Category GetCategory(int id)
        {
            try
            {
                var categoryEntity = _ecommerceUnitOfWork.Categories.GetById(id);
                var category = _mapper.Map<Category>(categoryEntity);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public IList<Category> GetCategories()
        {
            try
            {
                var categoryEntities = _ecommerceUnitOfWork.Categories.GetAll();
                List<Category> categories = new List<Category>();
                foreach (CategoryEntity entity in categoryEntities)
                {
                    categories.Add(_mapper.Map<Category>(entity));
                }
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public (int total, int totalDisplay, IList<Category> records) GetCategories(int pageIndex,
            int pageSize, string searchText, string orderBy)
        {
            List<Category> subCategories = new List<Category>();

            var result = _ecommerceUnitOfWork.Categories.GetDynamic
                (x => x.CategoryName != null && x.CategoryName.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);

            foreach (CategoryEntity entity in result.data)
            {
                subCategories.Add(_mapper.Map<Category>(entity));
            }
            return (result.total, result.totalDisplay, subCategories);
        }
        #endregion
    }
}
