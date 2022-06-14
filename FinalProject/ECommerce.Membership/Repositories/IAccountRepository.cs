using ECommerce.Core.Entities.Users;
using ECommerce.Membership.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Membership.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(RegisterModel model);
        Task<IdentityResult> CreateExternalUserAsync(ApplicationUser user);
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task SendEmailConfirmationEmail(string callbackUrl, string email);
        Task SendForgotPasswordEmail(string callbackUrl, string email);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<SignInResult> PasswordSignInAsync(LoginModel model);
        Task<IList<string>> GetCurrentUserRolesAsync(string email);
        Task RolesAsync(ApplicationUser user);
        Task ClaimAsync(ApplicationUser user);
        Task SignInAsync(string email);
        Task SignOutAsync();
        bool IsAuthenticated();
        string GetUserId();
        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);

        #region ManageAccount
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<bool> UpdateAccountAsync(ApplicationUser user);
        Task<bool> DeleteAccountAsync(string password);
        #endregion

        #region ExternalLogin
        AuthenticationProperties ConfigureExternalAuthentication(string provider, string returnUrl);
        #endregion
    }
}
