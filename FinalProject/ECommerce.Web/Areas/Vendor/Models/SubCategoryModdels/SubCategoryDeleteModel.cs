using ECommerce.Fascet.ForSubCategory;

namespace ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels
{
    public class SubCategoryDeleteModel
    {
        private ISubCategoryUnit _subCategoryUnit;

        public SubCategoryDeleteModel(ISubCategoryUnit subCategoryUnit)
        {
            _subCategoryUnit = subCategoryUnit;
        }

        internal async Task DeleteSubCategory(int id)
        {
            await _subCategoryUnit.DeleteServiceAsync(id);
        }
    }
}
