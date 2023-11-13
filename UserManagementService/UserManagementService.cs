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

    public async Task<UserModel> GetUser(string username)
    {
        return await db.GetUser(username);
    }

    public async Task UpdateUser<T>(string username, string attribute, T newValue)
    {
        await db.UpdateUser(username, attribute, newValue);
    }
}