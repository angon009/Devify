using ECommerce.Core.Entities.Stores;
using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities.Common
{
    public class StorePayments : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public Store Store { get; set; }
        public int StoreId { get; set; }
        public double PaymentAmount { get; set; }
        public string TransactionId { get; set; }
    }
}
