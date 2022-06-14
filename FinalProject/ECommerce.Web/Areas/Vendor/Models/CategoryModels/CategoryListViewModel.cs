using Autofac;
using AutoMapper;
using ECommerce.Infrastructure.Services.ForCategory;
using ECommerce.Web.Models;

namespace ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels
{
    public class CategoryListViewModel
    {
        public int Id { get; set; }
        public string CategoryName
        {
            get; set;
        }
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private ILifetimeScope _lifetimeScope;
        public CategoryListViewModel(ICategoryService categoryService,ILifetimeScope scope
            ,IMapper mapper)
        {
            _categoryService = categoryService;
            _lifetimeScope = scope;
            _mapper = mapper;
        }
        public CategoryListViewModel()
        {

        }
        public void Resolve(ILifetimeScope scope)
        {
            _categoryService = scope.Resolve<ICategoryService>() ;
            _lifetimeScope = scope.Resolve<ILifetimeScope>();
            _mapper = scope.Resolve<IMapper>() ;
        }

        //This method is call from ProductCreateViewModel.
        public async Task<List<CategoryListViewModel>> GetCategoriesAsync(int StoreId)
        {
            
            var categoryList = await _categoryService.GetCategoriesAsync(StoreId);
            List<CategoryListViewModel> categoryViewList = new();
            foreach (var category in categoryList)
            {
                categoryViewList.Add(_mapper.Map<CategoryListViewModel>(category));
            }
            return categoryViewList;
        }
        public async Task<object> GetPagedCategoriesAsync(DataTablesAjaxRequestModel model, int storeId)
        {
            var data = await _categoryService.GetCategoriesAsync(
                storeId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "CategoryName", "Description" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.CategoryName,
                                record.Description,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
