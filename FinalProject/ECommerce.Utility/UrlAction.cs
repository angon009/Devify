using Microsoft.AspNetCore.Http;

namespace ECommerce.Utility
{
    public static class UrlAction
    {
        public static string GetSubDomain()
        {
            var contextAccessor = new HttpContextAccessor();
            var host = contextAccessor.HttpContext.Request.Host.Value;

            // On development
            if (host.Contains("localhost"))
            {
                var subDomain = host.Split(".").First();
                return (subDomain != host) ? subDomain : null;
            }
            else
            {
                // On production
                if (host.StartsWith("www."))
                {
                    host = host.Replace("www.", "");
                }
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }

            return null;
        }
    }
}
