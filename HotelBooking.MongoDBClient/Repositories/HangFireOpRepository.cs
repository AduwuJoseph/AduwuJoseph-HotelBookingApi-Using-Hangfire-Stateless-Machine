using HotelBooking.MongoDBClient;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelBooking.MongoDBClient.Repositories
{
    public interface IHangFireOpRepository
    {
        HangFireOp Add(HangFireOp obj);
    }

    public class HangFireOpRepository : MongoDbRepository<HangFireOp>, IHangFireOpRepository
    {
        public HangFireOpRepository(IDatabaseContext databaseContext) : base(databaseContext) { }
        public HangFireOp Add(HangFireOp obj)
        {
            collection.InsertOne(obj);
            return obj;
        }
    }

    public class HangFireOp
    {
        public HangFireOp(string message)
        {
            Message = message;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
