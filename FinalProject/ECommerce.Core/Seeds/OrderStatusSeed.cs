using ECommerce.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Seeds
{
    internal static class OrderStatusSeed
    {
        internal static OrderStatus[] Status
        {
            get
            {
                return new OrderStatus[]
                {
                    new OrderStatus
                    {
                        Id=1,
                        TypeName="Processing"
                    },
                    new OrderStatus
                    {
                        Id=2,
                        TypeName="Ondelivery"
                    },
                    new OrderStatus
                    {
                        Id=3,
                        TypeName="Delivered"
                    },
                    new OrderStatus
                    {
                        Id=4,
                        TypeName="Cancelled"
                    }
                };
            }
        }
    }
}
