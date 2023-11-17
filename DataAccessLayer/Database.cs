using Amazon.Runtime.Internal.Util;
using DataAccessLayer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SharpCompress.Common;
using System.Diagnostics;
using System.Xml.Linq;
using static MongoDB.Driver.WriteConcern;

namespace DataAccessLayer;

public class Database : IDatabase, IPriceControl, IInventoryControl, ILoyaltyCard, IPurchaseManagement, IUserManagement, IReportAndAnalysis
{
    public static MongoClient client;
    public static IMongoDatabase database;
    string connectionString = "mongodb://localhost:27017/";
    string databaseName = "DE-Store_db";

    // Database setup
    public void DatabaseInitialisation()
    {
        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            database.CreateCollection("Products");
            database.CreateCollection("Users");
            database.CreateCollection("Transactions");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task SetProduct(string name, double price, int stock)
    {
        if (string.IsNullOrEmpty(name)) { throw new FormatException("Name provided is empty"); }
        if (double.IsNaN(price)) { throw new NotFiniteNumberException("Price provided is not a number"); }
        if (stock.GetType() != typeof(int)) { throw new NotFiniteNumberException("Stock provided is not a whole number"); }

        try
        {
            var collection = database.GetCollection<SharedModels.ProductModel>("Products");
            var entry = new SharedModels.ProductModel(name, price, stock);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Product with name {name} has been successfully inserted");
    }

    public async Task<SharedModels.ProductModel> GetProduct<T>(string findBy, T value)
    {
        if (findBy.ToLower() != "name" && findBy.ToLower() != "price" && findBy.ToLower() != "stock") { throw new FormatException($"{findBy} is not a valid parameter"); }

        var collection = database.GetCollection<SharedModels.ProductModel>("Products");

        try
        {
            if (findBy.ToLower() == "name" && value is string)
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("Name", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new SharedModels.ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if(findBy.ToLower() == "price" && value is double || value is int)
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("price", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new SharedModels.ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if (findBy.ToLower() == "stock" && value is int)
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("stock", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new SharedModels.ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else
            {
                return new SharedModels.ProductModel("", 0.0, 0);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new SharedModels.ProductModel("", 0.0, 0);
    }

    public async Task<List<SharedModels.ProductModel>> GetAllProducts()
    {
        var collection = database.GetCollection<SharedModels.ProductModel>("Products");

        try
        {
            var documents = await collection.FindAsync(_ => true);
            var results = await documents.ToListAsync();

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new List<SharedModels.ProductModel> { };
    }

    public async Task DeleteProduct(string name)
    {
        if (string.IsNullOrEmpty(name)) { throw new FormatException("Name provided is empty"); }

        var filter = Builders<SharedModels.ProductModel>.Filter.Eq("Name", name);

        try
        {
            var collection = database.GetCollection<SharedModels.ProductModel>("Products");

            collection.DeleteOne(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine($"Product with name {name} has been successfully deleted");
    }

    public async Task UpdateProduct<T>(string name, string attibute, T newValue)
    {
        if (attibute.ToLower() != "name" && attibute.ToLower() != "price" && attibute.ToLower() != "stock") { throw new FormatException($"{attibute} is not a valid parameter"); }

        var collection = database.GetCollection<SharedModels.ProductModel>("Products");

        try
        {
            if (attibute.ToLower() == "name")
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("Name", name);
                var update = Builders<SharedModels.ProductModel>.Update.Set("Name", newValue);
                collection.UpdateOne(filter, update);
            }
            else if (attibute.ToLower() == "price")
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("Name", name);
                var update = Builders<SharedModels.ProductModel>.Update.Set("Price", newValue);
                collection.UpdateOne(filter, update);
            }
            else if (attibute.ToLower() == "stock")
            {
                var filter = Builders<SharedModels.ProductModel>.Filter.Eq("Name", name);
                var update = Builders<SharedModels.ProductModel>.Update.Set("Stock", newValue);
                collection.UpdateOne(filter, update);
            }
            else
            {
                throw new FormatException($"{attibute} is not a valid parameter");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) { throw new FormatException("credenttials provided are not valid"); }

        try
        {
            var collection = database.GetCollection<SharedModels.UserModel>("Users");
            var entry = new SharedModels.UserModel(isAdmin, username, password, haveLoyaltyCard);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"User {username} has been successfully created");
    }

    public async Task<SharedModels.UserModel> GetUser(string username)
    {
        if (string.IsNullOrEmpty(username)) { throw new FormatException($"{username} is not a valid username"); }

        var collection = database.GetCollection<SharedModels.UserModel>("Users");

        try
        {
            var filter = Builders<SharedModels.UserModel>.Filter.Eq("Username", username);
            var documents = await collection.FindAsync(filter);
            var results = await documents.ToListAsync();

            return results.Select(document => new SharedModels.UserModel(
                document.IsAdmin,
                document.Username,
                document.Password,
                document.HaveLoyaltyCard
            )).FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new SharedModels.UserModel();
    }

    public async Task<List<SharedModels.UserModel>> GetAllUsers()
    {
        var collection = database.GetCollection<SharedModels.UserModel>("Users");

        try
        {
            var documents = await collection.FindAsync(_ => true);
            var results = await documents.ToListAsync();

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new List<SharedModels.UserModel> { };
    }

    public async Task UpdateUser<T>(string username, string attribute, T newValue)
    {
        if (string.IsNullOrEmpty(username)) { throw new FormatException($"{username} is not a valid username"); }
        if(attribute.ToLower() != "isadmin" && attribute.ToLower() != "username" && attribute.ToLower() != "password" && attribute.ToLower() != "haveloyaltycard") { throw new FormatException($"{attribute} is not a valid attribute"); }

        var collection = database.GetCollection<SharedModels.UserModel>("Users");

        try
        {
            var filter = Builders<SharedModels.UserModel>.Filter.Eq("Username", username);
            var update = Builders<SharedModels.UserModel>.Update.Set(attribute, newValue);
            collection.UpdateOne(filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task DeleteUser(string username)
    {
        if (string.IsNullOrEmpty(username)) { throw new FormatException("Username provided is empty"); }

        var filter = Builders<SharedModels.UserModel>.Filter.Eq("Username", username);

        try
        {
            var collection = database.GetCollection<SharedModels.UserModel>("Users");

            collection.DeleteOne(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine($"User with username {username} has been successfully deleted");
    }

    public async Task CreateTransaction(SharedModels.UserModel user, string product, int amount, bool buyNowPayLater)
    {
        try
        {
            var collection = database.GetCollection<SharedModels.TransactionModel>("Transactions");
            var entry = new SharedModels.TransactionModel(user, product, amount, buyNowPayLater);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Successfully purchased {amount} {product}");
    }

    public async Task<List<SharedModels.TransactionModel>> GetUserTransactions(SharedModels.UserModel user)
    {
        var collection = database.GetCollection<SharedModels.TransactionModel>("Transactions");

        try
        {
            var filter = Builders<SharedModels.TransactionModel>.Filter.Eq("User.Username", user.Username);
            var documents = await collection.FindAsync(filter);
            var results = await documents.ToListAsync();

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new List<SharedModels.TransactionModel>();
    }

    public async Task<List<SharedModels.TransactionModel>> GetAllTransactions()
    {
        var collection = database.GetCollection<SharedModels.TransactionModel>("Transactions");

        try
        {
            var documents = await collection.FindAsync(_ => true);
            var results = await documents.ToListAsync();

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new List<SharedModels.TransactionModel>();
    }
}