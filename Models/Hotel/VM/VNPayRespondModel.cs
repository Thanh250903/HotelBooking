namespace HotelApp.Models.Hotel.VM
{
    public class VNPayRespondModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayRespondCode { get; set; } // = 00 moi thanh cong dc
    }
    public class VNPayRequestModel
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }  
        public DateTime CreateDate { get; set; }

    }
}
