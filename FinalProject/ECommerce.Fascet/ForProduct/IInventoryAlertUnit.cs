using ECommerce.Infrastructure.BusinessObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Fascet.ForProduct
{
    public interface IInventoryAlertUnit
    {
        Task CreateOrUpdateServiceAsync(InventoryAlert inventory);
    }
}
