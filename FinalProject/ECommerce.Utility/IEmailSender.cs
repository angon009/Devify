namespace ECommerce.Utility
{
    public interface IEmailSender
    {
        Task SendAsync(string subject, string body, string receiverEmail, string receiverName);
    }
}
