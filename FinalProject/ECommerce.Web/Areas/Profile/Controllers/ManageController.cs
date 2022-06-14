using Autofac;
using ECommerce.Membership.Models;
using ECommerce.Membership.Repositories;
using ECommerce.Utility;
using ECommerce.Web.Areas.Profile.Models;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Customer.Controllers
{
    [ServiceFilter(typeof(StoreSubDomainChecker))]
    [Area("Profile")]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ManageController> _logger;

        public ManageController(IAccountRepository accountRepository,
            ILifetimeScope scope, ILogger<ManageController> logger)
        {
            _accountRepository = accountRepository;
            _scope = scope;
            _logger = logger;
        }
        public async Task<IActionResult> Message()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Settings()
        {
            var model = _scope.Resolve<SettingsModel>();
            try
            {
                model.GetTimeZones();
                await model.GetCurrentUserAync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsModel model)
        {
            try
            {
                model.Resolve(_scope);
                var result = await model.UpdateUser();
                if (result)
                {
                    // Profile update success --Tempdata
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Your profile has been updated successfully.",
                        Type = ResponseTypes.Success
                    });
                }
                else
                {
                    // Fail to update profile --Tempdata
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update your profile. Please try again.",
                        Type = ResponseTypes.Error
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Settings");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountRepository.ChangePasswordAsync(model);
                    if (result.Succeeded)
                    {
                        //ViewBag.IsSuccess = true;
                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "Password has been updated successfully.",
                            Type = ResponseTypes.Success
                        });
                        ModelState.Clear();
                        return RedirectToAction("Settings");
                    }

                    foreach (var error in result.Errors)
                    {
                        //ModelState.AddModelError("", error.Description);
                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = error.Description,
                            Type = ResponseTypes.Error
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _accountRepository.DeleteAccountAsync(password))
                    {
                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "Account has been deleted successfully.",
                            Type = ResponseTypes.Success
                        });
                        return RedirectToAction(typeof(Index).Name);

                    }
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to delete account. Please try again.",
                        Type = ResponseTypes.Error
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Settings");
        }
        public async Task<IActionResult> OrdersHistory()
        {
            var model = _scope.Resolve<OrderHistoryModel>();
            try
            {
                await model.GetOrders(_accountRepository.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }
        public async Task<IActionResult> CancelOrder(int id)
        {
            var model = _scope.Resolve<OrderHistoryModel>();
            try
            {
                await model.CancelOrder(id, _accountRepository.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction(nameof(OrdersHistory));
        }
    }
}
