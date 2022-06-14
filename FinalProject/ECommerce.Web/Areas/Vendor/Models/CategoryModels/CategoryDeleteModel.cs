using ECommerce.Fascet.ForCategory;

namespace ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels
{
    public class CategoryDeleteModel
    {
        private ICategoryUnit _categoryUnit;

        public CategoryDeleteModel(ICategoryUnit categoryUnit)
        {
            _categoryUnit = categoryUnit;
        }

        internal async Task DeleteCategory(int id)
        {
            await _categoryUnit.DeleteServiceAsync(id);
        }
    }
}
