namespace ECommerce.Utility
{
    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }
}
