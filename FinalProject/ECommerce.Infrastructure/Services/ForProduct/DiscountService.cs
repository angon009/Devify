using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountEntity = ECommerce.Core.Entities.Products.Discount;

namespace ECommerce.Infrastructure.Services.ForProduct
{
    public class DiscountService:IDiscountService
    {
        private readonly IMapper _mapper;
        private readonly IEcommerceUnitOfWork _ecommerceUnitOfWork;
        private readonly ILogger<DiscountService> _logger;
        public DiscountService(IMapper mapper,IEcommerceUnitOfWork ecommerceUnitOfWork,
            ILogger<DiscountService> logger)
        {
            _ecommerceUnitOfWork=ecommerceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task DeleteDiscountAsync(int Id)
        {
            try
            {
                await _ecommerceUnitOfWork.Discounts.RemoveAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task CreateDiscountAsync(Discount discount)
        {
            try
            {
                var entity = _mapper.Map<DiscountEntity>(discount);

                await _ecommerceUnitOfWork.Discounts.AddAsync(entity);
                await _ecommerceUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task UpdateDiscountAsync(Discount discount)
        {
            try
            {
                var discountEntity = await _ecommerceUnitOfWork.Discounts.GetByIdAsync(discount.Id);
                discountEntity = _mapper.Map(discount, discountEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<Discount> GetDiscountAsync(int id)
        {
            try
            {
                var discountEntity = await _ecommerceUnitOfWork.Discounts.GetByIdAsync(id);

                var discount = new Discount();
                discount = _mapper.Map<Discount>(discountEntity);

                return discount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<IList<Discount>> GetDiscountsAsync(int StoreId)
        {
            try
            {
                var discountEntities = await _ecommerceUnitOfWork.Discounts.GetAsync(x =>
                    x.StoreId == StoreId, string.Empty);

                var discounts = new List<Discount>();
                foreach (var discountEntity in discountEntities)
                {
                    var discount = new Discount();
                    discount = _mapper.Map<Discount>(discountEntity);
                    discounts.Add(discount);
                }

                return discounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<(int total, int totalDisplay, IList<Discount> records)>
            GetDiscountsAsync(int storeId, int pageIndex, int pageSize, string searchText, 
            string orderBy)
        {
            List<Discount> discounts = new List<Discount>();

            var result = await _ecommerceUnitOfWork.Discounts.GetDynamicAsync
                (x => x.DiscountName != null && x.DiscountName.Contains(searchText) && x.StoreId==storeId,
                orderBy, string.Empty, pageIndex, pageSize, true);
                foreach (DiscountEntity entity in result.data)
                {
                    discounts.Add(_mapper.Map<Discount>(entity));
                }
            return (result.total, result.totalDisplay, discounts);
        }
    }
}