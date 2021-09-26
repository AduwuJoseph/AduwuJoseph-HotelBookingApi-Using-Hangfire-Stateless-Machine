using HotelBooking.MongoDBClient.Enumerator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Models
{
    public class UserDVM
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string BirthDate { get; set; }

        public ESex Gender { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsConnected { get; set; }

        public PositionClass Position { get; set; }

        public class PositionClass
        {
            public double X { get; set; }

            public double Y { get; set; }
        }
    }
}
