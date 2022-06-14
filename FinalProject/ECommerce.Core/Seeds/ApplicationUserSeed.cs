using ECommerce.Core.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Seeds
{
    internal static class ApplicationUserSeed
    {
        internal static ApplicationUser[] Users
        {

            get
            {
                var user = new ApplicationUser
                {
                    Id = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                    FirstName = "Super Admin",
                    LastName = "",
                    UserName = "admin@ecommerce.com",
                    NormalizedUserName = "ADMIN@ECOMMERCE.COM",
                    Email = "admin@ecommerce.com",
                    NormalizedEmail = "ADMIN@ECOMMERCE.COM",
                    LockoutEnabled = true,
                    Gender = "Male",
                    //PasswordHash = "AQAAAAEAACcQAAAAEASXKSN8PtlyWFR9VGOd71qg3iyB/DOxE6mRQrc7c96j8G3VBTN9aVfVmkM+H19AWQ==",// Angon.00,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    Birthdate = DateTime.Now,
                    CreatedAt = DateTime.Now,

                };
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Angon.00");
                user.PasswordHash = hashed;
                return new ApplicationUser[]
                {
                    user

                };
            }
        }
    }
}
