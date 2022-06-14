namespace ECommerce.Utility.ChatHub
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(string message);
        Task ReceiveObject(MessagePayload payload);
    }
}
