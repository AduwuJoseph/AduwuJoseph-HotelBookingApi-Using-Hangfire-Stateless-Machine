using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Models
{
    public class HangfireEmailOp : Email
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Message { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}
