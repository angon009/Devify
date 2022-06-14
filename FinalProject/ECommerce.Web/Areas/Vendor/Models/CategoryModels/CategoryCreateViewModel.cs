using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForCategory;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForCategory;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels
{
    public class CategoryCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(20, ErrorMessage = "Name should be less than 20 chars")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        [StringLength(40, ErrorMessage = "Descripton should be less than 40 chars")]
        public string Description { get; set; }

        public int StoreId { get; set; }
        public CategoryCreateViewModel()
        {

        }


        private ICategoryUnit _categoryUnit;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public CategoryCreateViewModel(IMapper mapper, ICategoryUnit categoryUnit, ICategoryService categoryService)
        {
            _categoryUnit = categoryUnit;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryUnit = _scope.Resolve<ICategoryUnit>();
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        internal async Task CreateCategoryAsync()
        {
            var category = _mapper.Map<Category>(this);
            await _categoryUnit.CreateServiceAsync(category);
        }

        internal async Task LoadData(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            _mapper.Map(category, this);
        }

        internal async Task EditCategory()
        {
            var category = _mapper.Map<Category>(this);
            await _categoryUnit.UpdateServiceAsync(category);
        }
    }
}
