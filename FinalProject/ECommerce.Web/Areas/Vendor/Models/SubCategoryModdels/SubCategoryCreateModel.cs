using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForSubCategory;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForSubCategory;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels
{
    public class SubCategoryCreateModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Sub-Category Name")]
        [StringLength(30, ErrorMessage = "Name should less than 30 chars.")]
        public string SubCategoryName { get; set; }

        [Display(Name = "Sub-Category Description")]
        [StringLength(40, ErrorMessage = "Name should less than 40 chars.")]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }


        private ISubCategoryUnit _subCategoryUnit;
        private ISubCategoryService _subCategoryService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public SubCategoryCreateModel() { }
        public SubCategoryCreateModel(ISubCategoryUnit subCategoryUnit, ISubCategoryService subCategoryService, IMapper mapper)
        {
            _subCategoryUnit = subCategoryUnit;
            _subCategoryService = subCategoryService;
            _mapper = mapper;
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _subCategoryUnit = _scope.Resolve<ISubCategoryUnit>();
            _subCategoryService = _scope.Resolve<ISubCategoryService>();
            _mapper = _scope.Resolve<IMapper>();
        }
        internal async Task CreateSubCategoryAsync()
        {
            var subCategory = _mapper.Map<SubCategory>(this);
            await _subCategoryUnit.CreateServiceAsync(subCategory);
        }
        internal async Task LoadData(int id)
        {
            var subcategory = await _subCategoryService.GetSubCategoryAsync(id);
            _mapper.Map(subcategory, this);
        }
        internal async Task EditSubCategory()
        {
            var subCategory = _mapper.Map<SubCategory>(this);
            await _subCategoryUnit.UpdateServiceAsync(subCategory);
        }
    }
}
