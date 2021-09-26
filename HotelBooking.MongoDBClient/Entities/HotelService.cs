using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("Hotel_Services")]
    public class HotelService : Document
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Status { get; set; }
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());
    }
}
