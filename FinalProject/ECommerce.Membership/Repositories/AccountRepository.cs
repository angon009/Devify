using ECommerce.Core.Entities.Stores;
using ECommerce.Core.Entities.Users;
using ECommerce.Infrastructure.Services.ForAddress;
using ECommerce.Membership.Enums;
using ECommerce.Membership.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ECommerce.Membership.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ILogger<AccountRepository> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IActionContextAccessor _contextAccessor;
        private readonly IAddressService _addressService;
        private readonly IUrlHelper _urlHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountRepository(ILogger<AccountRepository> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor contextAccessor,
            IAddressService addressService)
        {

            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
            _addressService = addressService;
            _urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);

        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<IdentityResult> CreateUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = new List<Address>
                {
                     new Address
                    {
                        Division = model.Division,
                        District = model.District,
                        Thana = model.Thana,
                        PostOffice = null,
                        RoadNumber = model.RoadNumber
                    }
                },
                CreatedAt = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await RolesAsync(user);
                await GenerateEmailConfirmationTokenAsync(user);
            }

            return result;

        }
        public async Task<IdentityResult> CreateExternalUserAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await RolesAsync(user);
            }

            return result;
        }

        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = _urlHelper.Action(
            "ConfirmEmail",
            "Account",
            values: new
            {
                area = "",
                userId = user.Id,
                code = code,
            },
            protocol: _contextAccessor.ActionContext.HttpContext.Request.Scheme);

            await SendEmailConfirmationEmail(callbackUrl, user.Email);

        }
        public async Task SendEmailConfirmationEmail(string callbackUrl, string email)
        {

            await _emailSender.SendAsync("Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                email, email);
        }
        public async Task SignInAsync(string email)
        {
            await _signInManager.SignInAsync(await _userManager.FindByEmailAsync(email), isPersistent: false);
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task RolesAsync(ApplicationUser user)
        {
            var subDomain = UrlAction.GetSubDomain();
            var role = string.Empty;

            if (subDomain == null)
                role = Roles.Vendor.ToString();
            else
                role = Roles.Customer.ToString();

            await _userManager.AddToRolesAsync(user, new string[] { role });
        }
        public async Task ClaimAsync(ApplicationUser user)
        {
            await _userManager.AddClaimAsync(user, new Claim("ViewTestPage", "true"));
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result;
        }
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(LoginModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public AuthenticationProperties ConfigureExternalAuthentication(string provider, string returnUrl)
        {
            var redirectUrl = _urlHelper.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var result = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return result;
        }

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(code))
            {
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = _urlHelper.Action(
                    "ResetPassword",
                    "Account",
                    values: new { userid = user.Id, code },
                    protocol: _contextAccessor.ActionContext.HttpContext.Request.Scheme);

                await SendForgotPasswordEmail(callbackUrl, user.Email);
            }
        }

        public async Task SendForgotPasswordEmail(string callbackUrl, string email)
        {
            await _emailSender.SendAsync(
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", email, email);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.code, model.NewPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(GetUser);
            if(user != null)
            {
                var addresses = await _addressService.GetAddressAsync(user.Id);
                user.Address = addresses.ToList();
            }
            return user;
        }
        public string GetUserId()
        {
            return GetUser?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public ClaimsPrincipal GetUser => _contextAccessor.ActionContext.HttpContext.User;

        public bool IsAuthenticated()
        {
            return GetUser.Identity.IsAuthenticated;
        }

        public async Task<bool> UpdateAccountAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteAccountAsync(string password)
        {
            var user = await _userManager.GetUserAsync(GetUser);
            if (user == null)
            {
                throw new ArgumentNullException($"Unable to load user with ID '{_userManager.GetUserId(GetUser)}'.");
            }

            var RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, password))
                {
                    return false;
                    //ModelState.AddModelError(string.Empty, "Incorrect password.");
                    //return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return true;
        }

        public async Task<IList<string>> GetCurrentUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
        public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }
    }
}
