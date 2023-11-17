using DataAccessLayer;
using SharedModels;

namespace UserManagementService;

/* UserManagementService is a service that manages users */
public class UserManagementService : IUserManagementService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param isAdmin> Whether the user is an admin </param>
    /// <param username> The username of the user </param>
    /// <param password> The password of the user </param>
    /// <param haveLoyaltyCard> Whether the user has a loyalty card </param>
    public async Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard)
    {
        await db.CreateUser(isAdmin, username, password, haveLoyaltyCard);
    }

    /// <summary>
    /// Gets an user based on a username
    /// </summary>
    /// <param username> The username of the user </param>
    /// <returns> The retrieved user object </returns>
    public async Task<UserModel> GetUser(string username)
    {
        return await db.GetUser(username);
    }

    /// <summary>
    /// Gets all users present in the database
    /// </summary>
    /// <returns> A list of all present users </returns>
    public async Task<List<UserModel>> GetAllUsers()
    {
        return await db.GetAllUsers();
    }

    /// <summary>
    /// Updates a user's data
    /// </summary>
    /// <param username> The username of the user </param>
    /// <param attribute> The attribute that needs to be changed </param>
    /// <param newValue> The new value of that attribute </param>
    public async Task UpdateUser<T>(string username, string attribute, T newValue)
    {
        await db.UpdateUser(username, attribute, newValue);
    }
}