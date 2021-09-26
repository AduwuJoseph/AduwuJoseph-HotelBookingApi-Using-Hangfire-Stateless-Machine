using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Models
{
    public class CustomerDVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        public string Gender { get; set; }
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());

        public DateTime CreatedAt { get; set; }

    }
}
