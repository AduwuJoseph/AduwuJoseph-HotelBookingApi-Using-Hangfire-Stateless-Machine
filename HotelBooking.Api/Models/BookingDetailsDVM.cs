using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Api.Models
{
    public class BookingDetailsDVM
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BookingId { get; set; }
        public string ItemId { get; set; }
        public string ServiceType { get; set; } 
    }
}
