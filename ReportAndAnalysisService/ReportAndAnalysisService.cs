using DataAccessLayer;
using System.Reflection.Metadata.Ecma335;

namespace ReportAndAnalysisService;

public class ReportAndAnalysisService
{
    // Database variable to communicate with database
    DataAccessLayer.Database db = new DataAccessLayer.Database();

    public async Task<List<TransactionModel>> StoreAnalysis()
    {
        List<TransactionModel> transactions = await db.GetAllTransactions();
        return transactions;
    }
}