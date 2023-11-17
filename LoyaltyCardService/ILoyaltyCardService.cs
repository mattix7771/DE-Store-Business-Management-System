using SharedModels;

namespace LoyaltyCardService
{
    public interface ILoyaltyCardService
    {
        Task ApplyLoyaltyCard(string username);
        Task RemoveLoyaltyCard(string username);
        Task<List<UserModel>> LoyaltyCardHolders();
    }
}
