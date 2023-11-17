using SharedModels;

namespace PurchaseManagementService
{
    /* Interface for Purchase Management Service */
    public interface IPurchaseManagementService
    {
        Task MakePurchase(UserModel user, string product, int amount, bool buyNowPayLater);
        Task<List<TransactionModel>> GetUserPurchases(UserModel user);
    }
}
