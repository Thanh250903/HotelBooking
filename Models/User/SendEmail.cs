﻿namespace HotelApp.Models.User
{
    public class SendEmail
    {
        public string SmtpServer { get; set; }     
        public int SmtpPort { get; set; }          
        public string SenderName { get; set; }    
        public string SenderEmail { get; set; }    
        public string SenderPassword { get; set; }  
    }
}