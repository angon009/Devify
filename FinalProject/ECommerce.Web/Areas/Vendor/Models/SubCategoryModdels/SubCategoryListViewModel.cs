using Autofac;
using AutoMapper;
using ECommerce.Infrastructure.Services.ForCategory;
using ECommerce.Infrastructure.Services.ForSubCategory;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels
{
    public class SubCategoryListViewModel
    {
        private ISubCategoryService _subCategoryService;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public int Id { get; set; }
        public string SubCategoryName { get; set; }

        public SubCategoryListViewModel(ISubCategoryService subCategoryService, 
            ICategoryService categoryService,IMapper mapper,ILifetimeScope scope)
        {
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
            _mapper = mapper;
            _scope = scope;
        }

        internal async Task<object> GetCategory(int id)
        {
            var data = await _categoryService.GetCategoryAsync(id);
            return data;
        }
        public async Task<List<SubCategoryListViewModel>> GetSubCategoriesByCategoryIdAsync(int Id)
        {
            var list=await _subCategoryService.GetSubCategoriesAsync(Id);
            List<SubCategoryListViewModel> subCategories = new List<SubCategoryListViewModel>();
            foreach (var SubCategory in list)
            {
                SubCategoryListViewModel model = _scope.Resolve<SubCategoryListViewModel>();
                model = _mapper.Map(SubCategory, model);
                subCategories.Add(model);
            }
            return subCategories;
        }
        public async Task<object> GetPagedSubCategories(DataTablesAjaxRequestModel model, int id)
        {
            var data = await _subCategoryService.GetSubCategoriesAsync(
                id,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "SubCategoryName", "Description" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.SubCategoryName,
                                record.Description,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
