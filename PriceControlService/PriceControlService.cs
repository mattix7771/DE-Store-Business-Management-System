using DataAccessLayer;
using ServiceDiscovery;
using SharedModels;

namespace PriceControlService;

/* PriceControlService is a service that manages the price of products */
public class PriceControlService : IPriceControlService
{
    private readonly IServiceRegistry serviceRegistry;

    public PriceControlService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Sets a new price for a product
    /// </summary>
    /// <param productName> The name of the product whose price must be changes </param>
    /// <param productPrice> The new price of the product </param>
    public async Task SetProductPrice(string productName, double productPrice){

        // Get product entry from database
        IPriceControl db = serviceRegistry.GetService<IPriceControl>();
        ProductModel product = await db.GetProduct("name", productName);

        product.Price = productPrice;
        await db.UpdateProduct(productName, "price", productPrice);
    }

    /// <summary>
    /// Sets a new price for a product
    /// </summary>
    /// <param productName> The name of the product to retrieve from database </param>
    /// <returns> The price of the product </returns>
    public async Task<double> GetProductPrice(string productName){

        // Get product entry from database
        IPriceControl db = serviceRegistry.GetService<IPriceControl>();
        ProductModel product = await db.GetProduct("name", productName);

        return product.Price;
    }
}