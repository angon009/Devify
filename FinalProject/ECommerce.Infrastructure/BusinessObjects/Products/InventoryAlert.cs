using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.BusinessObjects.Products
{
    public class InventoryAlert
    {
        public int Id { get; set; }
        public int MinimumStock { get; set; }
        public int StoreId { get; set; }
        public string Status { get; set; }
    }
}
