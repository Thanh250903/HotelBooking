using HotelApp.Models.Others;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Hotel.VM
{
    public class PaymentVM
    {
        public int RoomBookingId { get; set; }
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        [Required]
        [CreditCard]
        public string CreditCard { get; set; }
        [Required]
        public string ExpirationDate { get; set; }
        [Required]
        public string CCV {  get; set; }
        public int TotalPrice { get; set; }

        //public Payment.PersonDetail PersonDetail { get; set; }
    }
}
