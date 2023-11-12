using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer;

public class TransactionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private ObjectId _id { get; set; }

    public TransactionModel()
    {
    }
}