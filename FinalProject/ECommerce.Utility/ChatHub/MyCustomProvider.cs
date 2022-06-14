using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Utility.ChatHub
{
    public class MyCustomProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public MyCustomProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

        }

        public string GetUserId(HubConnectionContext connection)
        {
            string? userEmail = _contextAccessor.HttpContext!.User.Identity!.Name;

            return userEmail!;
        }
    }
}
