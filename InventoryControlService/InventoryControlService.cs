using DataAccessLayer;
using SharedModels;

namespace InventoryControlService;

/* InventoryControlService is a service that manages stock of products, including:
 ordering new stock for products that are low on stock, monitor stock of products, and
generate warnings if a product has less than 10 units in stock*/
public class InventoryControlService : IInventoryControlService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    /// <summary>
    /// Orders stock for products
    /// </summary>
    /// <param productName> The name of the product whose stock must be ordered </param>
    /// <param stockOrdered> The stock ordered for the product </param>
    public async Task OrderStock(string productName, int stockOrdered)
    {
        Task<ProductModel> product = db.GetProduct("name", productName);

        int newStock = product.Result.Stock += stockOrdered;
        await db.UpdateProduct(productName, "stock", newStock);
    }

    /// <summary>
    /// Monitors product stock
    /// </summary>
    /// <param productName> The name of the product whose stock must be checked </param>
    /// <returns> The stock of a product </returns>
    public int MonitorStock(string productName)
    {
        Task<ProductModel> product = db.GetProduct("name", productName);

        return product.Result.Stock;
    }

    /// <summary>
    /// Generate warnings for products low on stock
    /// </summary>
    /// <returns> A list of products low on stock </returns>
    public async Task<List<ProductModel>> GenerateWarnings()
    {
        List<ProductModel> products = await db.GetAllProducts();
        List<ProductModel> productsLowStock = new List<ProductModel>();

        for (int i = 0; i < products.Count; i++)
        {
            ProductModel product = products[i];
            if(product.Stock < 10)
                productsLowStock.Add(product);
        }
        return productsLowStock;
    }

    /// <summary>
    /// Gets a list of all avaliable products
    /// </summary>
    /// <returns> a list of all avaliable products </returns>
    public async Task<List<ProductModel>> GetAllProducts()
    {
        List<ProductModel> products = await db.GetAllProducts();
        return products;
    }
}