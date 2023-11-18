using DataAccessLayer;
using ServiceDiscovery;
using SharedModels;

namespace LoyaltyCardService;

/* LoyaltyCardService is a service that manages user's loyalty cards, including:
 applying and revoking a loyalty card from a user, and getting all users with a loyalty card*/
public class LoyaltyCardService : ILoyaltyCardService
{
    // Registry setup to access relevant database actions
    private readonly IServiceRegistry serviceRegistry;
    public LoyaltyCardService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Applies a loyalty card to a user
    /// </summary>
    /// <param username> The username of the user </param>
    public async Task ApplyLoyaltyCard(string username)
    {
        try
        {
            // Get relevant database actions
            ILoyaltyCard db = serviceRegistry.GetService<ILoyaltyCard>();
            await db.UpdateUser(username, "HaveLoyaltyCard", true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Revokes a loyalty card from a user
    /// </summary>
    /// <param username> The username of the user </param>
    public async Task RemoveLoyaltyCard(string username)
    {
        try
        {
            // Get relevant database actions
            ILoyaltyCard db = serviceRegistry.GetService<ILoyaltyCard>();
            await db.UpdateUser(username, "HaveLoyaltyCard", false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all users who have a loyalty card
    /// </summary>
    /// <returns> A list of all users who have a loyalty card </returns>
    public async Task<List<SharedModels.UserModel>> LoyaltyCardHolders()
    {
        List<SharedModels.UserModel> loyaltyCardHolders = new List<SharedModels.UserModel>();
        try
        {
            // Get relevant database actions
            ILoyaltyCard db = serviceRegistry.GetService<ILoyaltyCard>();

            List<SharedModels.UserModel> allUsers = await db.GetAllUsers();
            foreach (SharedModels.UserModel user in allUsers)
            {
                if (user.HaveLoyaltyCard)
                    loyaltyCardHolders.Add(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return loyaltyCardHolders;
    }
}