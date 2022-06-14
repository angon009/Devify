using ECommerce.Core.Entities.Stores;

namespace ECommerce.Core.Seeds
{
    internal static class StoreStatusSeed
    {
        internal static StoreStatus[] Status
        {
            get
            {
                return new StoreStatus[]
                {
                    new StoreStatus
                    {
                        Id=1,
                        Status="Active"
                    },
                    new StoreStatus
                    {
                        Id=2,
                        Status="Inactive"
                    },
                    new StoreStatus
                    {
                        Id=3,
                        Status="Ondelete"
                    },
                    new StoreStatus
                    {
                        Id=4,
                        Status="Blocked"
                    }
                };
            }
        }
    }
}
