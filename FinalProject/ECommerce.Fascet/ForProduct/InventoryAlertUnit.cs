using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Fascet.ForProduct
{
    public class InventoryAlertUnit:IInventoryAlertUnit
    {
        private IInventoryAlertService _inventoryService;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        public InventoryAlertUnit(IInventoryAlertService inventoryService,IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _inventoryService = inventoryService;
            _ecommerceUnitOfWork=ecommerceUnitOfWork;
        }
        public async Task CreateOrUpdateServiceAsync(InventoryAlert inventory)
        {
            await _inventoryService.CreateOrUpdateInventoryAlertAsync(inventory);
            await _ecommerceUnitOfWork.SaveAsync();
        }
    }
}
