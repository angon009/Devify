using ECommerce.Core.Entities.MessageNotification;
using ECommerce.Core.Entities.Orders;
using ECommerce.Core.Entities.Stores;
using ECommerce.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Entities.Users
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public List<Address>? Address { get; set; }
        public List<Store>? Stores { get; set; }
        public DateTime Birthdate { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }
        public List<Cart>? Carts { get; set; }
        public List<Notification>? Notifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TimeZone { get; set; }
    }
}
