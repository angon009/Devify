using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using ECommerce.Web.PaymentGateWay;
using ECommerce.Utility;

namespace ECommerce.Web.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult CheckOut(double totalPrice, string confirmUrl, 
            string failedUrl, string cencelUrl, int storeId)
        {
            var baseUrl = Request.Scheme + "://" + Request.Host;

            NameValueCollection PostData = PostDataCollection.PostData(totalPrice, baseUrl, 
                confirmUrl, failedUrl, cencelUrl, storeId);
            TempData.Put("tran_id", PostData.Get("tran_id"));
            var sandboxStoreId = "ecomm6291bc340aea9";
            var storePassword = "ecomm6291bc340aea9@ssl";
            var isSandboxMood = true;

            SSLCommerz sslcz = new SSLCommerz(sandboxStoreId, storePassword, isSandboxMood);
            string response = sslcz.InitiateTransaction(PostData);

            return Redirect(response);
        }
    }
}
