using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("bookings")]
    public class Booking : Document
    {
        public string CustomerId { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
    }
}
