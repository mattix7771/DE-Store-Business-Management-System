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

public class Database
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
            var collection = database.GetCollection<ProductModel>("Products");
            var entry = new ProductModel(name, price, stock);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Product with name {name} has been successfully inserted");
    }

    public async Task<ProductModel> GetProduct<T>(string findBy, T value)
    {
        if (findBy.ToLower() != "name" && findBy.ToLower() != "price" && findBy.ToLower() != "stock") { throw new FormatException($"{findBy} is not a valid parameter"); }

        var collection = database.GetCollection<ProductModel>("Products");

        try
        {
            if (findBy.ToLower() == "name" && value is string)
            {
                var filter = Builders<ProductModel>.Filter.Eq("Name", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if(findBy.ToLower() == "price" && value is double || value is int)
            {
                var filter = Builders<ProductModel>.Filter.Eq("price", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if (findBy.ToLower() == "stock" && value is int)
            {
                var filter = Builders<ProductModel>.Filter.Eq("stock", value);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();

                return results.Select(document => new ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else
            {
                return new ProductModel("", 0.0, 0);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new ProductModel("", 0.0, 0);
    }

    public async Task<List<ProductModel>> GetAllProducts()
    {
        var collection = database.GetCollection<ProductModel>("Products");

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
        return new List<ProductModel> { };
    }

    public async Task DeleteProduct(string name)
    {
        if (string.IsNullOrEmpty(name)) { throw new FormatException("Name provided is empty"); }

        var filter = Builders<ProductModel>.Filter.Eq("Name", name);

        try
        {
            database.CreateCollection("Products");
            var collection = database.GetCollection<ProductModel>("Products");

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

        var collection = database.GetCollection<ProductModel>("Products");

        try
        {
            if (attibute.ToLower() == "name")
            {
                var filter = Builders<ProductModel>.Filter.Eq("Name", name);
                var update = Builders<ProductModel>.Update.Set("Name", newValue);
                collection.UpdateOne(filter, update);
            }
            else if (attibute.ToLower() == "price")
            {
                var filter = Builders<ProductModel>.Filter.Eq("Name", name);
                var update = Builders<ProductModel>.Update.Set("Price", newValue);
                collection.UpdateOne(filter, update);
            }
            else if (attibute.ToLower() == "stock")
            {
                var filter = Builders<ProductModel>.Filter.Eq("Name", name);
                var update = Builders<ProductModel>.Update.Set("Stock", newValue);
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
            database.CreateCollection("Users");
            var collection = database.GetCollection<UserModel>("Users");
            var entry = new UserModel(isAdmin, username, password, haveLoyaltyCard);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"User {username} has been successfully created");
    }

    public async Task<UserModel> GetUser(string username)
    {
        if (string.IsNullOrEmpty(username)) { throw new FormatException($"{username} is not a valid username"); }

        var collection = database.GetCollection<UserModel>("Users");

        try
        {
            var filter = Builders<UserModel>.Filter.Eq("username", username);
            var documents = await collection.FindAsync(filter);
            var results = await documents.ToListAsync();

            return results.Select(document => new UserModel(
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
        return new UserModel();
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        var collection = database.GetCollection<UserModel>("Users");

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
        return new List<UserModel> { };
    }

    public async Task UpdateUser<T>(string username, string attribute, T newValue)
    {
        if (string.IsNullOrEmpty(username)) { throw new FormatException($"{username} is not a valid username"); }
        if(attribute.ToLower() != "isadmin" && attribute.ToLower() != "username" && attribute.ToLower() != "password" && attribute.ToLower() != "haveloyaltycard") { throw new FormatException($"{attribute} is not a valid attribute"); }

        var collection = database.GetCollection<UserModel>("Users");

        try
        {
            var filter = Builders<UserModel>.Filter.Eq("Username", username);
            var update = Builders<UserModel>.Update.Set(attribute, newValue);
            collection.UpdateOne(filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task CreateTransaction(UserModel user, string product, int amount, bool buyNowPayLater)
    {
        try
        {
            database.CreateCollection("Transactions");
            var collection = database.GetCollection<TransactionModel>("Transactions");
            var entry = new TransactionModel(user, product, amount, buyNowPayLater);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine($"Successfully purhcased {amount} {product}");
    }

    public async Task<TransactionModel> GetTransaction(UserModel user)
    {
        var collection = database.GetCollection<TransactionModel>("Transactions");

        try
        {
            var filter = Builders<TransactionModel>.Filter.Eq("User", user);
            var documents = await collection.FindAsync(filter);
            var results = await documents.ToListAsync();

            return results.Select(document => new TransactionModel(
                document.User,
                document.Product,
                document.Amount,
                document.BuyNowPayLater
            )).FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new TransactionModel();
    }
}