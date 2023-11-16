using DataAccessLayer;

namespace LoyaltyCardService;

/* LoyaltyCardService is a service that manages user's loyalty cards, including:
 applying and revoking a loyalty card from a user, and getting all users with a loyalty card*/
public class LoyaltyCardService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    public async Task ApplyLoyaltyCard(string username)
    {
        try
        {
            await db.UpdateUser(username, "HaveLoyaltyCard", true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task RemoveLoyaltyCard(string username)
    {
        try
        {
            await db.UpdateUser(username, "HaveLoyaltyCard", false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task<List<UserModel>> LoyaltyCardHolders()
    {
        List<UserModel> loyaltyCardHolders = new List<UserModel>();
        try
        {
            List<UserModel> allUsers = await db.GetAllUsers();
            foreach (UserModel user in allUsers)
            {
                if(user.HaveLoyaltyCard)
                    loyaltyCardHolders.Add(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return loyaltyCardHolders;
    }