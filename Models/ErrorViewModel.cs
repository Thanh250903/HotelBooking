namespace HotelApp.Models
{
    public class ErrorViewModel
    {
        internal bool showRequestId;

        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
