using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Api.Models
{
    public class RoomDVM
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Status { get; set; }
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());
    }
}
