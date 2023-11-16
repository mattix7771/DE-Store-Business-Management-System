using DataAccessLayer;
using FinanceApprovalService;

namespace PurchaseManagementService;

public class PurchaseManagementService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();
    
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

    public async Task<List<TransactionModel>> GetUserPurchases(UserModel user)
    {
        List<TransactionModel> transactions = await db.GetUserTransactions(user);
        return transactions;
    }
}