using HotelBooking.MongoDBClient.Enumerator;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("bookings")]
    public class BookingDetails : Document
    {
        public string ItemId { get; set; }
        public string ServiceType { get; set; } 
    }
}
