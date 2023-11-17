using SharedModels;

namespace InventoryControlService
{
    public interface IInventoryControlService
    {
        Task OrderStock(string productName, int stockOrdered);
        int MonitorStock(string productName);
        Task<List<ProductModel>> GenerateWarnings();
        Task<List<ProductModel>> GetAllProducts();
        Task DeleteProduct (string productName);
    }
}