namespace ECommerce.Web.Models
{
    public enum ResponseTypes
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class ResponseModel
    {
        public string? Message { get; set; }
        public ResponseTypes Type { get; set; }
    }
}
