using Autofac;
using ECommerce.Fascet.ForProduct;

namespace ECommerce.Web.Areas.Vendor.Models.StockModels
{
    public class StockUpdateModel
    {
        private IProductUnit _productUnit;
        private ILifetimeScope _scope;

        public string? ProductName { get; set; }
        public int? Quantity { get; set; } 
        public StockUpdateModel(IProductUnit productUnit)
        {
            _productUnit = productUnit;
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _productUnit = _scope.Resolve<IProductUnit>();
        }
        public async Task UpdateQuantityAsync(int id, int quantity)
        {
            await _productUnit.UpdateQuantityServiceAsync(id,quantity);
        }
    }
}
