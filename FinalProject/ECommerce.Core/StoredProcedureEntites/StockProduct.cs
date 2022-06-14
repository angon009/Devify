using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.StoredProcedureEntites
{
    public class StockProduct
    {
        public StockProduct()
        {

        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? saleprice { get; set; }
        public string? brand { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? Quantity { get; set; }
    }
}
