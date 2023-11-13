using DataAccessLayer;

namespace LoyaltyCardService;

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

    //GetLoyaltyOffers
}