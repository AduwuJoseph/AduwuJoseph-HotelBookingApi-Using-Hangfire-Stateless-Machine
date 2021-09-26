using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("Email_Histories")]
    public class EmailHistory : Document
    {
        public string Message { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}
