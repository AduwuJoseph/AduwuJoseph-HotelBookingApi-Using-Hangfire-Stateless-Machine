using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Models
{
    public class DashboardData
    {
        public long TotalRooms { get; set; }
        public long FreeRooms { get; set; }
        public int TotalCheckIns { get; set; }
        public int TotalCheckOuts { get; set; }

        public decimal Revenue { get; set; }

    }
}
