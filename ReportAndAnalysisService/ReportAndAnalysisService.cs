using DataAccessLayer;
using ServiceDiscovery;
using SharedModels;

namespace ReportAndAnalysisService;

/* ReportAndAnalysisService is a service that produces valuable statistics about the store */
public class ReportAndAnalysisService : IReportAndAnalysisService
{
    // Registry setup to access relevant database actions
    private readonly IServiceRegistry serviceRegistry;
    public ReportAndAnalysisService(IServiceRegistry serviceRegistry)
    {
        this.serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// Produces a short analysis of the store's transactions
    /// </summary>
    /// <returns> A list of all recorded transactions </returns>
    public async Task<List<TransactionModel>> StoreAnalysis()
    {
        List<TransactionModel> transactions = new List<TransactionModel>();
        try
        {
            // Get relevant database actions
            IReportAndAnalysis db = serviceRegistry.GetService<IReportAndAnalysis>();

            transactions = await db.GetAllTransactions();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return transactions;
    }
}