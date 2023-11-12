using Amazon.Runtime.Internal.Util;
using DataAccessLayer;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

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
            database.CreateCollection("Products");
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
}