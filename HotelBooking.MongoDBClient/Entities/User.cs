using HotelBooking.MongoDBClient.Enumerator;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.MongoDBClient.Entities
{
    [BsonCollection("Users")]
    public class User :Document
    {
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Position { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; }
        public bool IsConnected { get; set; } = false;
        public bool IsInBlackList { get; set; } = false;
        public DateTime? BirthDate { get; set; } = null;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public DateTime RegistrerDate { get; set; } = DateTime.Now;
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());

        public User()
        {
        }
     }

}
