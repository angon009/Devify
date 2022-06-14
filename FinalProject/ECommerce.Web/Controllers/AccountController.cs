using Autofac;
using ECommerce.Core.Entities.Users;
using ECommerce.Membership.Models;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    [ServiceFilter(typeof(StoreSubDomainChecker))]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ILifetimeScope _scope;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ILogger<AccountController> logger,
            ILifetimeScope scope,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _scope = scope;
            _userManager = userManager;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var model = _scope.Resolve<RegisterModel>();
            try
            {
                model.ReturnUrl = returnUrl;
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.CreateUserAsync(model);

                    if (!result.Succeeded)
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }

                    ModelState.Clear();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("ConfirmEmail", new { email = model.Email });
                    }
                    else
                    {
                        await _accountRepository.SignInAsync(model.Email);
                        var roles = await _accountRepository.GetCurrentUserRolesAsync(model.Email);
                        if (roles.Contains("Vendor"))
                        {
                            model.ReturnUrl = Url.Action("Stores", "Dashboard", new { area = "Vendor" });
                        }
                        else
                        {
                            model.ReturnUrl ??= Url.Content("~/Store");
                        }
                        return LocalRedirect(model.ReturnUrl!);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string email)
        {
            var model = _scope.Resolve<EmailConfirmModel>();
            model.Email = email;

            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(code))
                {
                    var result = await _accountRepository.ConfirmEmailAsync(userId, code);

                    if (result.Succeeded)
                    {
                        model.EmailVerified = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            try
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        model.EmailVerified = true;
                        return View(model);
                    }

                    await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                    model.EmailSent = true;
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var model = _scope.Resolve<LoginModel>();

            try
            {
                if (!string.IsNullOrEmpty(model.ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, model.ErrorMessage);
                }

                returnUrl ??= Url.Content("~/");

                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                model.ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {

            try
            {
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.PasswordSignInAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        var roles = await _accountRepository.GetCurrentUserRolesAsync(model.Email);
                        if (roles.Contains("Admin"))
                        {
                            model.ReturnUrl ??= Url.Content("~/Admin/Dashboard/Index");
                        }
                        else if (roles.Contains("Vendor"))
                        {
                            model.ReturnUrl ??= Url.Action("Stores", "Dashboard", new { area = "Vendor" });
                        }
                        else
                        {
                            model.ReturnUrl ??= Url.Content("~/Store");
                        }
                        await GetCartSession(model.Email);
                        return LocalRedirect(model.ReturnUrl!);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            try
            {
                await _accountRepository.SignOutAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            TempData.Clear();
            return RedirectToAction(typeof(Index).Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var properties = _accountRepository.ConfigureExternalAuthentication(provider, returnUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null,
            string remoteError = null)
        {
            var model = _scope.Resolve<RegisterModel>();
            returnUrl = returnUrl ?? Url.Content("~/");
            model.ReturnUrl = returnUrl;
            model.ExternalLogins =
                (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(String.Empty, "Error loading external login information during confirmation.");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
             
                return LocalRedirect(model.ReturnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                if (email != null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = name
                    };

                    var result = await _accountRepository.CreateExternalUserAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {

                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            var roles = await _accountRepository.GetCurrentUserRolesAsync(email);
                            if (roles.Contains("Vendor"))
                            {
                                model.ReturnUrl ??= Url.Content("~/Vendor/Dashboard/Stores");
                            }
                            else
                            {
                                model.ReturnUrl ??= returnUrl;
                            }
                            return LocalRedirect(model.ReturnUrl!);
                        }
                    }
                }

            }

            return View();
        }
        public async Task GetCartSession(string email)
        {
            var model = _scope.Resolve<ShoppingCartModel>();
            var applicationUser = await _accountRepository.GetUserByEmailAsync(email);
            var cartItemList = await model.GetCartAsync(applicationUser.Id);

            var cartSession = new List<CartItemModel>();

            if (TempData.Peek<IList<CartItemModel>>("CartSession") != null &&
                TempData.Peek<IList<CartItemModel>>("CartSession").Count > 0)
            {
                cartSession = TempData.Get<IList<CartItemModel>>("CartSession").ToList();

            }

            cartItemList.ForEach(x =>
            {
                var existItem = cartSession.FirstOrDefault(c => c.ProductId == x.ProductId);
                if (existItem != null)
                {
                    existItem.Id = x.Id;
                    existItem.Quantity += x.Quantity;
                }
                else
                {
                    cartSession.Add(x);
                }
            });

            TempData.Put("CartSession", cartSession);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // code here
                    var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                    if (user != null)
                    {
                        await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                    }

                    ModelState.Clear();
                    model.EmailSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string code)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _scope.Resolve<ResetPasswordModel>();
                    model.code = code;
                    model.UserId = userId;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.ResetPasswordAsync(model);
                    if (result.Succeeded)
                    {
                        ModelState.Clear();
                        model.IsSuccess = true;
                        return View(model);
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Login");
        }
    }
}