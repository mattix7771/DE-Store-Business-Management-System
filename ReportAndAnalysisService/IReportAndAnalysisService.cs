using SharedModels;

namespace ReportAndAnalysisService
{
    public interface IReportAndAnalysisService
    {
        Task<List<TransactionModel>> StoreAnalysis();
    }
}
