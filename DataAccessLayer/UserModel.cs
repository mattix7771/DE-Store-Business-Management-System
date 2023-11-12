using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer;

public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private ObjectId _id;
    public ObjectId Id { get => _id; set => _id = value; }
    
    private bool _isAdmin;
    public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

    private string _username;
    public string Username { get => _username; set => _username = value; }

    private string _password;
    public string Password { get => _password; set => _password = value; }

    private bool _haveLoyaltyCard;
    public bool HaveLoyaltyCard { get => _haveLoyaltyCard; set => _haveLoyaltyCard = value;}

    public UserModel() { }
    public UserModel(bool isAdmin, string username, string password, bool haveLoyaltyCard)
    {
        this._isAdmin = isAdmin;
        this._username = username;
        this._password = password;
        this._haveLoyaltyCard = haveLoyaltyCard;
    }
}