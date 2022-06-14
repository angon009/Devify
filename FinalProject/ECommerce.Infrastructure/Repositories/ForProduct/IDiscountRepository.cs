using ECommerce.Core.Entities.Products;
using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public interface IDiscountRepository:IRepository<Discount, int>
    {
    }
}
