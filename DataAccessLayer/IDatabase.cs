namespace DataAccessLayer
{
    /* Interface for Data Access Layer */
    public interface IDatabase
    {
        void DatabaseInitialisation();
        Task SetProduct(string name, double price, int stock);
        Task<SharedModels.ProductModel> GetProduct<T>(string findBy, T value);
        Task<List<SharedModels.ProductModel>> GetAllProducts();
        Task DeleteProduct(string name);
        Task UpdateProduct<T>(string name, string attribute, T newValue);

        Task CreateUser(bool isAdmin, string username, string password, bool haveLoyaltyCard);
        Task<SharedModels.UserModel> GetUser(string username);
        Task<List<SharedModels.UserModel>> GetAllUsers();
        Task UpdateUser<T>(string username, string attribute, T newValue);

        Task CreateTransaction(SharedModels.UserModel user, string product, int amount, bool buyNowPayLater);
        Task<List<SharedModels.TransactionModel>> GetUserTransactions(SharedModels.UserModel user);
        Task<List<SharedModels.TransactionModel>> GetAllTransactions();
    }
}