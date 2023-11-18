namespace DataAccessLayer
{
    /* Interface for Data Access Layer */
    public interface IDatabase
    {
        void DatabaseInitialisation();
    }
    public interface IPriceControl
    {
        Task SetProduct(string name, double price, int stock);
        Task UpdateProduct<T>(string name, string attribute, T newValue);
        Task<SharedModels.ProductModel> GetProduct<T>(string findBy, T value);
    }
    public interface IInventoryControl
    {
        Task<SharedModels.ProductModel> GetProduct<T>(string findBy, T value);
        Task UpdateProduct<T>(string name, string attribute, T newValue);
        Task<List<SharedModels.ProductModel>> GetAllProducts();
        Task DeleteProduct(string productName);
    }
    public interface ILoyaltyCard
    {
        Task UpdateUser<T>(string username, string attribute, T newValue);
        Task<List<SharedModels.UserModel>> GetAllUsers();
    }
    public interface IPurchaseManagement
    {
        Task CreateTransaction(SharedModels.UserModel user, string product, int amount, bool buyNowPayLater);
        Task UpdateProduct<T>(string name, string attribute, T newValue);
        Task<List<SharedModels.TransactionModel>> GetUserTransactions(SharedModels.UserModel user);
        Task<SharedModels.ProductModel> GetProduct<T>(string findBy, T value);
    }
    public interface IUserManagement
    {
        Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard);
        Task<SharedModels.UserModel> GetUser(string username);
        Task<List<SharedModels.UserModel>> GetAllUsers();
        Task UpdateUser<T>(string username, string attribute, T newValue);
        Task DeleteUser(string username);
    }
    public interface IReportAndAnalysis
    {
        Task<List<SharedModels.TransactionModel>> GetAllTransactions();
    }
}