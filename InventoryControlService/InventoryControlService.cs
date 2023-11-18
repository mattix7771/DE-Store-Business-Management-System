using DataAccessLayer;
using ServiceDiscovery;
using SharedModels;

namespace InventoryControlService;

/* InventoryControlService is a service that manages stock of products, including:
 ordering new stock for products that are low on stock, monitor stock of products, and
generate warnings if a product has less than 10 units in stock*/
public class InventoryControlService : IInventoryControlService
{
    // Registry setup to access relevant database actions
    private readonly IServiceRegistry serviceRegistry;
    public InventoryControlService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Orders stock for products
    /// </summary>
    /// <param productName> The name of the product whose stock must be ordered </param>
    /// <param stockOrdered> The stock ordered for the product </param>
    public async Task OrderStock(string productName, int stockOrdered)
    {
        try
        {
            // Get relevant database actions
            IInventoryControl db = serviceRegistry.GetService<IInventoryControl>();

            ProductModel product = await db.GetProduct("name", productName);

            int newStock = product.Stock += stockOrdered;
            await db.UpdateProduct(productName, "stock", newStock);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Monitors product stock
    /// </summary>
    /// <param productName> The name of the product whose stock must be checked </param>
    /// <returns> The stock of a product </returns>
    public async Task<int> MonitorStock(string productName)
    {
        ProductModel product = null;
        try
        {
            // Get relevant database actions
            IInventoryControl db = serviceRegistry.GetService<IInventoryControl>();

            product = await db.GetProduct("name", productName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return product.Stock;
    }

    /// <summary>
    /// Generate warnings for products low on stock
    /// </summary>
    /// <returns> A list of products low on stock </returns>
    public async Task<List<ProductModel>> GenerateWarnings()
    {
        List<ProductModel> productsLowStock = new List<ProductModel>();

        try
        {
            // Get relevant database actions
            IInventoryControl db = serviceRegistry.GetService<IInventoryControl>();

            List<ProductModel> products = await db.GetAllProducts();

            for (int i = 0; i < products.Count; i++)
            {
                ProductModel product = products[i];
                if(product.Stock < 10)
                    productsLowStock.Add(product);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return productsLowStock;
    }

    /// <summary>
    /// Gets a list of all avaliable products
    /// </summary>
    /// <returns> a list of all avaliable products </returns>
    public async Task<List<ProductModel>> GetAllProducts()
    {
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            // Get relevant database actions
            IInventoryControl db = serviceRegistry.GetService<IInventoryControl>();

            products = await db.GetAllProducts();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        return products;
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param productName> The name of the product whose stock must be checked </param>
    public async Task DeleteProduct(string productName)
    {
        try
        {
            // Get relevant database actions
            IInventoryControl db = serviceRegistry.GetService<IInventoryControl>();

            await db.DeleteProduct(productName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}