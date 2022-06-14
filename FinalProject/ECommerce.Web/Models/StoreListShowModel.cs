using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.BusinessObjects.Common;

namespace ECommerce.Web.Models
{
    public class StoreListShowModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public string StatusId { get; set; }
        public string Address { get; set; }
        public Image StoreImage { get; set; }
    }
}
