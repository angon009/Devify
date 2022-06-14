using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Membership.Models
{
    public class RegisterModel
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Division")]
        public string Division { get; set; }

        [Required]
        [Display(Name = "District")]
        public string District { get; set; }

        [Required]
        [Display(Name = "Thana")]
        public string Thana { get; set; }

        //[Required]
        //[Display(Name = "Post Office")]
        //public string? PostOffice { get; set; }

        [Required]
        [Display(Name = "Road Number")]
        public string RoadNumber { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
