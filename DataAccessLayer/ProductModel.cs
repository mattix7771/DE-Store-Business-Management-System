using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZstdSharp.Unsafe;

namespace DataAccessLayer;

public class ProductModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    //[BsonElement("Id")]
    private ObjectId _id;
    public ObjectId Id { get => _id; set => _id = value; }

    //[BsonElement("Name")]
    private string _name;
    public string Name { get => _name; set => _name = value; }
    //[BsonElement("Price")]
    private double _price;
    public double Price { get => _price; set => _price = value; }
    //[BsonElement("Stock")]
    private int _stock;
    public int Stock { get => _stock; set => _stock = value; }

    public ProductModel(string name, double price, int stock)
    {
        this._name = name;
        this._price = price;
        this._stock = stock;
    }
}