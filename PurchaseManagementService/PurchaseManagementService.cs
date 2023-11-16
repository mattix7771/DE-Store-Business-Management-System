using DataAccessLayer;
using FinanceApprovalService;

namespace PurchaseManagementService;

/* PurchaseManagementService is a service that manages purchases and transactions */
public class PurchaseManagementService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    /// <summary>
    /// Creates a purchase transaction and adjusts corresponding stock values
    /// </summary>
    /// <param user> User who has made the purchase </param>
    /// <param product> The product purchased </param>
    /// <param amount> The amount purchased </param>
    /// <param buyNowPayLater> Whether the user has applied for the buy now pay later service </param>
    public async Task MakePurchase(UserModel user, string product, int amount, bool buyNowPayLater)
    {
        if (buyNowPayLater)
        {
            FinanceApprovalService.FinanceApprovalService financeApprovalService= new FinanceApprovalService.FinanceApprovalService();
            financeApprovalService.ApproveFinanace();
        }
        await db.CreateTransaction(user, product, amount, buyNowPayLater);
        ProductModel getProduct = await db.GetProduct("Name", product);
        await db.UpdateProduct(product, "Stock", getProduct.Stock - amount);
    }

    /// <summary>
    /// Gets all purchases made by a user
    /// </summary>
    /// <param user> User who has made the purchase </param>
    /// <returns> A list of all the purchases made by an user </returns>
    public async Task<List<TransactionModel>> GetUserPurchases(UserModel user)
    {
        List<TransactionModel> transactions = await db.GetUserTransactions(user);
        return transactions;
    }
}