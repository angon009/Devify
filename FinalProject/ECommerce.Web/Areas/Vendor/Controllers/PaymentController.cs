using ECommerce.Web.Areas.Vendor.Models.BillPayModels;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Web.Models;
using ECommerce.Web.ControllerLevelValidation;

namespace ECommerce.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ILifetimeScope _scope;
        private ILogger<PaymentController> _logger;
        public PaymentController(ILifetimeScope scope, ILogger<PaymentController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        [StoreIdChecker]
        public async Task<IActionResult> Index()
        {
            var model = new StorePaymentModel();
            return View(model);
        }

        public async Task<JsonResult> GetPayments()
        {
            try
            {
                int storeId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
                var dataTableModel = new DataTablesAjaxRequestModel(Request);
                var model = _scope.Resolve<StorePaymentModel>();
                var list = await model.GetPagedPayments(dataTableModel, storeId);
                return Json(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MakePayment(StorePaymentModel model)
        {
            var xstoreId = TempData.Peek<StoreDetailsViewModel>("StoreInfo").Id;
            return RedirectToAction("CheckOut", "Billing",
                new { Area = "", totalPrice = 3000, confirmUrl  = model.ConfirmUrl,
                    failedUrl = model.FailedUrl, cencelUrl = model.CencelUrl, 
                    storeId = xstoreId });
        }

        [AllowAnonymous]
        public async Task<IActionResult> PaymentConfirmed(Guid tran_id,double totalPrice, int storeId)
        {
            ViewBag.TransactionId = tran_id.ToString();
            ViewBag.TotalPrice = totalPrice;
            CreateStorePaymentModel model = new CreateStorePaymentModel
            {
                TransactionId = tran_id.ToString(),
                PaymentDate = DateTime.Now,
                PaymentAmount = totalPrice,
                StoreId = storeId
            };

            model.Resolve(_scope);
            try
            {
                await model.CreatePaymentAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult PaymentFailed()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult PaymentCenceled()
        {
            return View();
        }
    }
}