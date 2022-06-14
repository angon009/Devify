using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities.Products
{
    public class InventoryAlert : IEntity<int>
    {
        public int Id { get; set; }
        public int MinimumStock { get; set; }
        public int StoreId { get; set; }
        public string Status { get; set; }
    }
}
