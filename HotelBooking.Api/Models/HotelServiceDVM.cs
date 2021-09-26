using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Models
{
    public class HotelServiceDVM
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Status { get; set; }
        public BsonArray Pictures { get; set; } = new BsonArray(new List<string>());

        public DateTime CreatedAt { get; set; }
    }
}
