using DataAccessLayer;

namespace PriceControlService;

/* PriceControlService is a service that manages the price of products */
public class PriceControlService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    /// <summary>
    /// Sets a new price for a product
    /// </summary>
    /// <param productName> The name of the product whose price must be changes </param>
    /// <param productPrice> The new price of the product </param>
    public async Task SetProductPrice(String productName, double productPrice){
        
        // Get product entry from database
        Task<ProductModel> product = db.GetProduct("name", productName);

        product.Result.Price = productPrice;
        await db.UpdateProduct(productName, "price", productPrice);
    }

    /// <summary>
    /// Sets a new price for a product
    /// </summary>
    /// <param productName> The name of the product to retrieve from database </param>
    /// <returns> The price of the product </returns>
    public double GetProductPrice(String productName){

        // Get product entry from database
        Task<ProductModel> product = db.GetProduct("name", productName);

        return product.Result.Price;
    }
}