using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Infrastructures
{
    public static class DatabaseSettings 
    {
        public static string ConnectionString { get; set; } = "mongodb://localhost:27017/";
        public static string DatabaseName { get; set; } = "HotelBookingDB";
    }

}
