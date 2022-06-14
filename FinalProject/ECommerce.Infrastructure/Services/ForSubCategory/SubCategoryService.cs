using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using SubCategoryEntity = ECommerce.Core.Entities.Products.SubCategory;

namespace ECommerce.Infrastructure.Services.ForSubCategory
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryService> _logger;

        public SubCategoryService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper,
            ILogger<SubCategoryService> logger)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region Asynchronus Methods
        public async Task CreateSubCategoryAsync(SubCategory subCategory)
        {
            try
            {
                var subCategoryCount = await _ecommerceUnitOfWork.SubCategories
                        .GetCountAsync(s => s.SubCategoryName == subCategory.SubCategoryName);

                if (subCategoryCount == 0)
                {
                    var entity = _mapper.Map<SubCategoryEntity>(subCategory);

                    await _ecommerceUnitOfWork.SubCategories.AddAsync(entity);
                }
                else
                {
                    throw new DuplicateDataException("SubCategory with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UpdateSubCategoryAsync(SubCategory subCategory)
        {
            try
            {
                var subCategoryCount = await _ecommerceUnitOfWork.SubCategories
                        .GetCountAsync(s => s.SubCategoryName == subCategory.SubCategoryName &&
                        s.Id != subCategory.Id);

                if (subCategoryCount == 0)
                {
                    var subCategoryEntity = await _ecommerceUnitOfWork.SubCategories
                        .GetByIdAsync(subCategory.Id);

                    await Task.Run(() =>
                    {
                        subCategoryEntity = _mapper.Map(subCategory, subCategoryEntity);
                    });
                }
                else
                {
                    throw new DuplicateDataException("SubCategory with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task DeleteSubCategoryAsync(int id)
        {
            try
            {
                await _ecommerceUnitOfWork.SubCategories.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<SubCategory> GetSubCategoryAsync(int id)
        {
            try
            {
                var subCategoryEntity = await _ecommerceUnitOfWork.SubCategories.GetByIdAsync(id);

                SubCategory subCategory = new SubCategory();

                await Task.Run(() =>
                {
                    subCategory = _mapper.Map<SubCategory>(subCategoryEntity);
                });

                return subCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<IList<SubCategory>> GetSubCategoriesAsync(int CategoryId)
        {
            try
            {
                var subCategoryEntities = await _ecommerceUnitOfWork.SubCategories.GetAsync(x
                        => x.CategoryId == CategoryId, string.Empty);

                List<SubCategory> subCategories = new List<SubCategory>();

                foreach (SubCategoryEntity entity in subCategoryEntities)
                {
                    subCategories.Add(_mapper.Map<SubCategory>(entity));
                }
                return subCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(int total, int totalDisplay, IList<SubCategory> records)> 
            GetSubCategoriesAsync(int categoryId, int pageIndex, int pageSize, 
            string searchText, string orderBy)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            var result = await _ecommerceUnitOfWork.SubCategories.GetDynamicAsync
                (x => x.SubCategoryName != null && x.CategoryId == categoryId &&
                x.SubCategoryName.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);

            await Task.Run(() =>
            {
                foreach (SubCategoryEntity entity in result.data)
                {
                    subCategories.Add(_mapper.Map<SubCategory>(entity));
                }
            });
            return (result.total, result.totalDisplay, subCategories);
        }
        #endregion

        #region Non-Asynchronus Methods
        public void CreateSubCategory(SubCategory subCategory)
        {
            try
            {
                var subCategoryCount = _ecommerceUnitOfWork.SubCategories
                        .GetCount(s => s.SubCategoryName == subCategory.SubCategoryName);

                if (subCategoryCount == 0)
                {
                    var entity = _mapper.Map<SubCategoryEntity>(subCategory);

                    _ecommerceUnitOfWork.SubCategories.Add(entity);
                }
                else
                {
                    throw new DuplicateDataException("SubCategory with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            try
            {
                var subCategoryCount = _ecommerceUnitOfWork.SubCategories
                        .GetCount(s => s.SubCategoryName == subCategory.SubCategoryName &&
                        s.Id == subCategory.Id);

                if (subCategoryCount == 0)
                {
                    var subCategoryEntity = _ecommerceUnitOfWork.SubCategories
                        .GetById(subCategory.Id);

                    subCategoryEntity = _mapper.Map(subCategory, subCategoryEntity);
                }
                else
                {
                    throw new DuplicateDataException("SubCategory with same name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void DeleteSubCategory(int id)
        {
            try
            {
                _ecommerceUnitOfWork.SubCategories.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public SubCategory GetSubCategory(int id)
        {
            try
            {
                var subCategoryEntity = _ecommerceUnitOfWork.SubCategories.GetById(id);
                var subCategory = _mapper.Map<SubCategory>(subCategoryEntity);
                return subCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public IList<SubCategory> GetSubCategories(int CategoryId)
        {
            try
            {
                var subCategoryEntities = _ecommerceUnitOfWork.SubCategories.GetAll()
                        .Where(x => x.CategoryId == CategoryId).ToList();

                List<SubCategory> subCategories = new List<SubCategory>();

                foreach (SubCategoryEntity entity in subCategoryEntities)
                {
                    subCategories.Add(_mapper.Map<SubCategory>(entity));
                }
                return subCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public (int total, int totalDisplay, IList<SubCategory> records) 
            GetSubCategories(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            var result = _ecommerceUnitOfWork.SubCategories.GetDynamic
                (x => x.SubCategoryName != null && x.SubCategoryName.Contains(searchText),
                orderBy, string.Empty, pageIndex, pageSize, true);

            foreach (SubCategoryEntity entity in result.data)
            {
                subCategories.Add(_mapper.Map<SubCategory>(entity));
            }

            return (result.total, result.totalDisplay, subCategories);
        }
        #endregion
    }
}
