using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Api.Models
{
    public class BookingDVM
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal TotalCost { get; set; }
        public string Status { get; set; }

        public bool IsBooked { get; set; } = false;

        public List<BookingDetailsDVM> BookingDetails { get; set; } = new List<BookingDetailsDVM>();
    }
}
