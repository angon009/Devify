using ECommerce.Utility;
using ECommerce.Web.Areas.StoreAdmin.Models.StoreModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Web.ControllerLevelValidation
{
    public class StoreIdChecker:ActionFilterAttribute
    {
        
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            Controller controller =  actionContext.Controller as Controller;

             if(controller != null)
            {
                if (controller.TempData.Peek<StoreDetailsViewModel>("StoreInfo") == null)
                {
                    actionContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        Action = "Stores",
                        Controller = "Dashboard",
                        Area="Vendor"
                    })
                    );
                };
            }
            

            base.OnActionExecuted(actionContext);
        }
    }
}
