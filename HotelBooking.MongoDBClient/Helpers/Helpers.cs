using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Helpers
{
    public static class BookServiceType
    {
        public static string Room = "Room";
        public static string Service = "Service";
    }
    public static class BookingStatus
    {
        public static string Room = "Room";
        public static string Service = "Service";
        public static string Registered = "Registered";
        public static string BookedRoom = "BookedRoom";
        public static string CheckIn = "CheckIn";
        public static string CheckOut = "CheckOut";
        public static string Free = "Free";
        public static string Occupied = "Occupied";
        public static string Reserved = "Reserved";
        public static string Cleaned = "Cleaned";
        public static string Unavailable = "Unavailable";
    }
}
