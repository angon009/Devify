using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Utility
{
    public class StoreSubDomainChecker : ActionFilterAttribute
    {

        private IEcommerceUnitOfWork _ecommerceUnitOfWork;

        public StoreSubDomainChecker(IEcommerceUnitOfWork ecommerceUnitOfWork)
        {
            _ecommerceUnitOfWork = ecommerceUnitOfWork;
        }

        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {

            var subDomain = UrlAction.GetSubDomain();

            if (subDomain != null)
            {
                var storeCount = _ecommerceUnitOfWork.Stores
                     .GetCount(s => s.SubDomain!.ToLower() == subDomain.ToLower() && s.StoreStatusId == 1);
                if (storeCount == 0)
                {
                    actionContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new
                        {
                            Action = "Index",
                            Controller = "Error"
                        })
                        );
                }
            }


            base.OnActionExecuted(actionContext);
        }

    }
}
