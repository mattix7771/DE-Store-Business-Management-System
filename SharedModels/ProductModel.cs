using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModels;

public class ProductModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private ObjectId _id;
    public ObjectId Id { get => _id; set => _id = value; }

    private string _name;
    public string Name { get => _name; set => _name = value; }
    private double _price;
    public double Price { get => _price; set => _price = value; }
    private int _stock;
    public int Stock { get => _stock; set => _stock = value; }

    private string _offer;
    public string Offer { get => _offer; set => _offer = value; }

    public ProductModel(string name, double price, int stock)
    {
        this._name = name;
        this._price = price;
        this._stock = stock;
    }
    public ProductModel(string name, double price, int stock, string offer)
    {
        this._name = name;
        this._price = price;
        this._stock = stock;
        this._offer = offer;
    }
}