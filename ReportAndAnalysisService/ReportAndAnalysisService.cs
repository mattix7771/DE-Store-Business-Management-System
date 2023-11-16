using DataAccessLayer;
using System.Reflection.Metadata.Ecma335;

namespace ReportAndAnalysisService;

/* ReportAndAnalysisService is a service that produces valuable statistics about the store */
public class ReportAndAnalysisService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    /// <summary>
    /// Produces a short analysis of the store's transactions
    /// </summary>
    /// <returns> A list of all recorded transactions </returns>
    public async Task<List<TransactionModel>> StoreAnalysis()
    {
        List<TransactionModel> transactions = await db.GetAllTransactions();
        return transactions;
    }
}