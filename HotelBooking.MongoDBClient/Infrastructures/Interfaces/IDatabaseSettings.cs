using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.MongoDBClient.Infrastructures.Interfaces
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
