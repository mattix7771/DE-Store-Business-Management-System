using DataAccessLayer;

namespace UserManagementService;

public class UserManagementService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    public async Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard)
    {
        await db.CreateUser(isAdmin, username, password, haveLoyaltyCard);
    }
}