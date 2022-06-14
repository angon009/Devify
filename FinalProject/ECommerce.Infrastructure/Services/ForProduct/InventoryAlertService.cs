using AutoMapper;
using ECommerce.Core.StoredProcedureEntites;
using ECommerce.Core.StoredProcedureEntites.SqlEnitities;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryAlertEntity = ECommerce.Core.Entities.Products.InventoryAlert;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public class InventoryAlertService:IInventoryAlertService
    {
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly IMapper _mapper;

        public InventoryAlertService(IEcommerceUnitOfWork ecommerceUnitOfWork, IMapper mapper)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
            _mapper = mapper;
        }
        public async Task CreateOrUpdateInventoryAlertAsync(InventoryAlert inventory)
        {
            var entity=_mapper.Map<InventoryAlertEntity>(inventory);
            var count = await _ecommerceUnitOfWork.InventoryAlerts.GetCountAsync(x => x.StoreId == inventory.StoreId);
            if (count > 0)
            {
                var getEntity =(await _ecommerceUnitOfWork.InventoryAlerts.GetAsync(x => x.StoreId == inventory.StoreId, string.Empty))[0];
                getEntity.StoreId = inventory.StoreId;
                getEntity.Status = inventory.Status;
                getEntity.MinimumStock = inventory.MinimumStock;
                
                
            }
            else
            {
                await _ecommerceUnitOfWork.InventoryAlerts.AddAsync(entity);
            }
        }
        public async Task<InventoryAlert> GetInventoryAlertByStoreIdAsync(int storeId)
        {
            int count = await _ecommerceUnitOfWork.InventoryAlerts.GetCountAsync(x => x.StoreId == storeId);
            if (count > 0)
            {
                var entity = (await _ecommerceUnitOfWork.InventoryAlerts.GetAsync(x => x.StoreId == storeId, string.Empty))[0];
                var returnEntity = _mapper.Map<InventoryAlert>(entity);
                return returnEntity;
            }
            return  new InventoryAlert();
            
        }

        public async Task<(int total, int totalDisplay, IList<StockProduct> records)> GetInventoryAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId,
            int MinimumQuantity)
        {
            var result=await _ecommerceUnitOfWork.InventoryAlerts.GetInventoryProductsAsync(pageIndex
                , pageSize, orderBy, storeId, MinimumQuantity);
            List<StockProduct> products = new List<StockProduct>();
            foreach (var product in result.data)
            {
                products.Add(product);
            }

            return (result.total, result.totalFiltered, products);
        }
        public async Task<(int total, int totalDisplay, IList<StockProduct> records)> GetInventoryAlertAsync(
            int pageIndex,
            int pageSize,
            string orderBy,
            int storeId)
        {
            int count = await _ecommerceUnitOfWork.InventoryAlerts.GetCountAsync(x => x.StoreId == storeId);
            int minQuantity = 1000;
            if (count > 0)
            {
                var entity = (await _ecommerceUnitOfWork.InventoryAlerts.GetAsync(x => x.StoreId == storeId, string.Empty))[0];
                minQuantity = entity.MinimumStock;
            }
            var result = await _ecommerceUnitOfWork.InventoryAlerts.GetInventoryProductsAsync(pageIndex
                , pageSize, orderBy, storeId, minQuantity);
            List<StockProduct> products = new List<StockProduct>();
            foreach (var product in result.data)
            {
                products.Add(product);
            }

            return (result.total, result.totalFiltered, products);
        }

        public async Task<(int TotalProducts,int TotalOrders,int TotalSales)> GetDashboardValues(int StoreId)
        {
            var result = await _ecommerceUnitOfWork.InventoryAlerts.GetDashboardValues(StoreId);
            List<int> total = new List<int>();  
            foreach (var item in result)
            {
                total.Add(item.DashboardValue);
            }
            return (total[0], total[1], total[2]);
        }
    }
}
