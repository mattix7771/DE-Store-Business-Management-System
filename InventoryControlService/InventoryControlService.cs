using DataAccessLayer;
using MongoDB.Driver;

namespace InventoryControlService;

public class InventoryControlService
{
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    //reorderstock, monitorstock, generatewarnings
    public async Task OrderStock(string productName, int stockOrdered)
    {
        Task<ProductModel> product = db.GetProduct("name", productName);

        int newStock = product.Result.Stock += stockOrdered;
        await db.UpdateProduct(productName, "stock", newStock);
    }

    public int MonitorStock(string productName)
    {
        Task<ProductModel> product = db.GetProduct("name", productName);

        return product.Result.Stock;
    }

    public void GenerateWarnings()
    {

    }
}