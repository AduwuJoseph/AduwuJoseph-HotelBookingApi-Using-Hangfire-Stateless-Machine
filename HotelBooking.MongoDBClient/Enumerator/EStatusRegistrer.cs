using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Enumerator
{
  public enum EStatusRegistrer
  {
    OK                = 300,
    USER_EXIST        = 301,
    EMAIL_EXIST       = 302
  }
}
