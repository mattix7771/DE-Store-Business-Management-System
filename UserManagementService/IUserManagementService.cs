using SharedModels;

namespace UserManagementService
{
    /* Interface for User Management Service */
    public interface IUserManagementService
    {
        Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard);
        Task<UserModel> GetUser(string username);
        Task<List<UserModel>> GetAllUsers();
        Task UpdateUser<T>(string username, string attribute, T newValue);
        Task DeleteUser(string username);
    }
}
