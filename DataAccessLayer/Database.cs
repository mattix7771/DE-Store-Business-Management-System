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
    public async Task DatabaseInitialisationAsync()
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
            var collection = database.GetCollection<ProductModel>("productsCollection");
            var entry = new ProductModel(name, price, stock);
            await collection.InsertOneAsync(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task<ProductModel> GetProduct(string? name = null, double? price = null, int? stock = null, string findBy = "")
    {
        //if (string.IsNullOrEmpty(name)) { throw new FormatException("Name provided is empty"); }
        //if (double.IsNaN(price)) { throw new NotFiniteNumberException("Price provided is not a number"); }
        //if (stock.GetType() != typeof(int)) { throw new NotFiniteNumberException("Stock provided is not a whole number"); }
        if (findBy.ToLower() != "name" || findBy.ToLower() != "price" || findBy.ToLower() != "stock" || findBy != "") { throw new FormatException($"{findBy} is not a valid parameter"); }


        ProductModel model;
        try
        {
            var collection = database.GetCollection<ProductModel>("productsCollection");

            if (findBy.ToLower() == "name")
            {
                var filter = Builders<ProductModel>.Filter.Eq("name", name);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();
                return results.Select(document => new ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if(findBy.ToLower() == "price")
            {
                var filter = Builders<ProductModel>.Filter.Eq("price", price);
                var documents = await collection.FindAsync(filter);
                var results = await documents.ToListAsync();
                return results.Select(document => new ProductModel(
                    document.Name,
                    document.Price,
                    document.Stock
                )).FirstOrDefault();
            }
            else if (findBy.ToLower() == "stock")
            {
                var filter = Builders<ProductModel>.Filter.Eq("stock", stock);
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
}