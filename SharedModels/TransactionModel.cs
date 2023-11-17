using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModels;

public class TransactionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private ObjectId _id { get; set; }

    private UserModel _user;
    public UserModel User { get => _user; set => _user = value; }

    private string _product;
    public string Product { get => _product; set => _product = value; }

    private int _amount;
    public int Amount { get => _amount; set => _amount = value; }
    
    private bool _buyNowPayLater;
    public bool BuyNowPayLater { get => _buyNowPayLater; set => _buyNowPayLater = value; }

    public TransactionModel() { }
    public TransactionModel(UserModel user, string product, int amount, bool buyNowPayLater)
    {
        _user = user;
        _product = product;
        _amount = amount;
        _buyNowPayLater = buyNowPayLater;
    }
}