using ECommerce.Core.Entities.Users;

namespace ECommerce.Core.Seeds
{
    public static class RoleSeed
    {
        public static Role[] Roles
        {
            get
            {
                return new Role[]
                {
                    new Role{ Id = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7210"), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp =  DateTime.Now.Ticks.ToString()  },
                    new Role{ Id = Guid.NewGuid(), Name = "Vendor", NormalizedName = "VENDOR", ConcurrencyStamp =  DateTime.Now.AddMinutes(1).Ticks.ToString()  },
                    new Role{ Id = Guid.NewGuid(), Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp =  DateTime.Now.AddMinutes(2).Ticks.ToString()  }
                };
            }
        }
    }
}
