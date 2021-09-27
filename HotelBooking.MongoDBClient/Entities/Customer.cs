using HotelBooking.MongoDBClient.Enumerator;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("customers")]
    public class Customer : Document
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        public string Gender { get; set; }
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());
    }
}
