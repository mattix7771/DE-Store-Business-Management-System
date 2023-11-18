using DataAccessLayer;
using ServiceDiscovery;
using FinanceApprovalService;
using SharedModels;

namespace PurchaseManagementService;

/* PurchaseManagementService is a service that manages purchases and transactions */
public class PurchaseManagementService : IPurchaseManagementService
{
    // Registry setup to access relevant database actions
    private readonly IServiceRegistry serviceRegistry;
    public PurchaseManagementService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Creates a purchase transaction and adjusts corresponding stock values
    /// </summary>
    /// <param user> User who has made the purchase </param>
    /// <param product> The product purchased </param>
    /// <param amount> The amount purchased </param>
    /// <param buyNowPayLater> Whether the user has applied for the buy now pay later service </param>
    public async Task MakePurchase(UserModel user, string product, int amount, bool buyNowPayLater)
    {
        try
        {
            // Get relevant database actions
            IPurchaseManagement db = serviceRegistry.GetService<IPurchaseManagement>();

            if (buyNowPayLater)
            {
                FinanceApprovalService.FinanceApprovalService financeApprovalService= new FinanceApprovalService.FinanceApprovalService();
                financeApprovalService.ApproveFinance();
            }
            await db.CreateTransaction(user, product, amount, buyNowPayLater);
            ProductModel getProduct = await db.GetProduct("Name", product);
            await db.UpdateProduct(product, "Stock", getProduct.Stock - amount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all purchases made by a user
    /// </summary>
    /// <param user> User who has made the purchase </param>
    /// <returns> A list of all the purchases made by an user </returns>
    public async Task<List<TransactionModel>> GetUserPurchases(UserModel user)
    {
        List<TransactionModel> transactions = new List<TransactionModel>();
        try
        {
            // Get relevant database actions
            IPurchaseManagement db = serviceRegistry.GetService<IPurchaseManagement>();

            transactions = await db.GetUserTransactions(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return transactions;
    }
}