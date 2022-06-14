using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Web.Models;
using ECommerce.Utility;
using ECommerce.Membership.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Autofac;
using ECommerce.Web.Areas.Vendor.Models.ForDiscount;

namespace ECommerce.Web.Areas.StoreAdmin.Models.ProductModels
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int[] ListOfProductId { get; set; }
        public int DiscountId { get; set; }
        public List<DiscountListViewModel>? discountList { get; set; }
        public List<SelectListItem> products = new List<SelectListItem>();
        private IProductService _productService;
        private IProductUnit _productUnit;
        private ILifetimeScope _scope;
        private IAccountRepository _accountRepository;
        public ProductListViewModel()
        {

        }
        public ProductListViewModel(IProductService productService
            ,IProductUnit productUnit, IAccountRepository accountRepository, ILifetimeScope scope)
        {
            _productService = productService;
            _productUnit = productUnit;
            _accountRepository = accountRepository;
            _scope = scope; 
        }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _productService = _scope.Resolve<IProductService>();
            _productUnit = _scope.Resolve<IProductUnit>();
            _accountRepository = _scope.Resolve<IAccountRepository>();
        }
        
            
        public async Task LoadDataToProducts(int storeId)
        {
            var result=await _productService.GetProductsByStoreIdAsync(storeId);
            foreach (var item in result)
            {
                var selectone = new SelectListItem();
                selectone.Text = item.Name;
                selectone.Value = item.Id.ToString();
                products.Add(selectone);
            }
        }
        public async Task AssignDiscounttoProducts()
        {
           await _productService.DiscountAssignToMultiProducts(this.ListOfProductId, this.DiscountId);
        }
        public async Task DeleteProduct(int Id)
        {
            await _productUnit.DeleteServiceAsync(Id);
        }
        public async Task<object> GetPagedStores(DataTablesAjaxRequestModel model,int StoreId)
        {
            var user = await _accountRepository.GetCurrentUserAsync();
            var data =await _productService.GetProductsAsync(
                StoreId,
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] {"Name","Brand","SalePrice","CostPrice","Quantity","Color","ExpireDate","ManufactureDate" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name,
                                record!.Brand!,
                                record.SalePrice.ToString(),
                                record.CostPrice.ToString(),
                                record.Quantity.ToString(),
                                record!.Color!,
                                record!.ExpireDate!.CurrentZone(user.TimeZone!).ToString()!,
                                record!.ManufactureDate!.CurrentZone(user.TimeZone!).ToString()!,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
