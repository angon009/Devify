using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Utility.ChatHub
{

    public class ChatHub : Hub
    {

        public async Task SendMessage(string sender, string receiver, string message)
        {
            string[] receivers = new string[] { sender, receiver };
            if (string.IsNullOrEmpty(receiver))
                await Clients.All.SendAsync("ReceiveMessageHandler", sender, message, receiver);
            else
                await Clients.Users(receivers).SendAsync("ReceiveMessageHandler", message, sender, receiver);
        }
    }
}
