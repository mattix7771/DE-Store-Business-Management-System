using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer;

public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private ObjectId _id { get; set; }

    [BsonElement("Name")]
    private string _name { get; set; }
    [BsonElement("Type")]
    private string _type { set; get; }

    public UserModel(string name, string type)
    {
        this._name = name;
        this._type = type;
    }
}