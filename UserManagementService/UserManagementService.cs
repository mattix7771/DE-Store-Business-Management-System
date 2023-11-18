using DataAccessLayer;
using ServiceDiscovery;
using SharedModels;

namespace UserManagementService;

/* UserManagementService is a service that manages users */
public class UserManagementService : IUserManagementService
{
    // Registry setup to access relevant database actions
    private readonly IServiceRegistry serviceRegistry;
    public UserManagementService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param isAdmin> Whether the user is an admin </param>
    /// <param username> The username of the user </param>
    /// <param password> The password of the user </param>
    /// <param haveLoyaltyCard> Whether the user has a loyalty card </param>
    public async Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard)
    {
        try
        {
            // Get relevant database actions
            IUserManagement db = serviceRegistry.GetService<IUserManagement>();

            await db.CreateUser(isAdmin, username, password, haveLoyaltyCard);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets an user based on a username
    /// </summary>
    /// <param username> The username of the user </param>
    /// <returns> The retrieved user object </returns>
    public async Task<UserModel> GetUser(string username)
    {
        UserModel user = null;
        try
        {
            // Get relevant database actions
            IUserManagement db = serviceRegistry.GetService<IUserManagement>();

            user = await db.GetUser(username);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return user;
    }

    /// <summary>
    /// Gets all users present in the database
    /// </summary>
    /// <returns> A list of all present users </returns>
    public async Task<List<UserModel>> GetAllUsers()
    {
        List<UserModel> allUsers = new List<UserModel>();
        try
        {
            // Get relevant database actions
            IUserManagement db = serviceRegistry.GetService<IUserManagement>();

            allUsers = await db.GetAllUsers();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return allUsers;
    }

    /// <summary>
    /// Updates a user's data
    /// </summary>
    /// <param username> The username of the user </param>
    /// <param attribute> The attribute that needs to be changed </param>
    /// <param newValue> The new value of that attribute </param>
    public async Task UpdateUser<T>(string username, string attribute, T newValue)
    {
        try
        {
            // Get relevant database actions
            IUserManagement db = serviceRegistry.GetService<IUserManagement>();

            await db.UpdateUser(username, attribute, newValue);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param username> The username of the user </param>
    public async Task DeleteUser(string username)
    {
        try
        {
            // Get relevant database actions
            IUserManagement db = serviceRegistry.GetService<IUserManagement>();

            await db.DeleteUser(username);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}