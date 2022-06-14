using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class CustomerList
    {
        public CustomerList()
        {

        } 
        public string? FirstName { get; set; } 
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; } 
        public double? TotalAmount { get; set; } 
        public double? TotalDiscount { get; set; } 
        public int? TotalOrders { get; set; }
        public string? Address { get; set; }   


    }
}
