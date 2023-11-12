using DataAccessLayer;

namespace PriceControlService;

public class PriceControlService
{
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    //get and set product price
    public async Task SetProductPrice(String productName, double productPrice){
        
        Task<ProductModel> product = db.GetProduct("name", productName);

        product.Result.Price = productPrice;

        await db.DeleteProduct(productName);
        Console.WriteLine($"Product set: Name = {product.Result.Name}, Price = {product.Result.Price}, Stock = {product.Result.Stock}");
        await db.SetProduct(product.Result.Name, product.Result.Price, product.Result.Stock);
    }

    public double GetProductPrice(String productName){

        Task<ProductModel> product = db.GetProduct("name", productName);

        return product.Result.Price;
    }

}