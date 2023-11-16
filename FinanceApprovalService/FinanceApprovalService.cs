namespace FinanceApprovalService;

/* FinanceApprovalService is a service that approves a buy now pay later purchase
 * This service would connect to the "Enabling" portal which would verify or deny the purchase*/
public class FinanceApprovalService
{
    /// <summary>
    /// Verifies a buy now pay later purchase
    /// </summary>
    public void ApproveFinanace()
    {
        Console.WriteLine("Connecting to Enabling Financing System...");
        Thread.Sleep(3000);
        Console.WriteLine("Purchase Approved");
    }
}