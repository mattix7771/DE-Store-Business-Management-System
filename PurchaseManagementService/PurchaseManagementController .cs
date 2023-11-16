//using DataAccessLayer;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace PurchaseManagementService
//{
//    public class PurchaseManagementController
//    {
//        private readonly HttpClient _financeApprovalClient;

//        public PurchaseManagementController()
//        {
//            _financeApprovalClient = new HttpClient();
//            _financeApprovalClient.BaseAddress = new Uri("http://localhost:12345/"); // Adjust the URL accordingly
//        }

//        public async Task<string> MakePurchase(PurchaseRequest request)
//        {
//            // Purchase logic
//            // ...

//            // If Buy Now Pay Later is enabled, call FinanceApprovalService
//            if (request.BuyNowPayLater)
//            {
//                await CallFinanceApprovalService(request.User, request.Product, request.Amount);
//            }

//            // Continue with the purchase logic
//            // ...

//            return "Purchase successful";
//        }

//        private async Task CallFinanceApprovalService(UserModel user, string product, int amount)
//        {
//            var financeApprovalRequest = new FinanceApprovalRequest
//            {
//                User = user,
//                Product = product,
//                Amount = amount
//            };

//            // Make an HTTP request to FinanceApprovalService
//            var response = await _financeApprovalClient.PostAsJsonAsync("api/FinanceApproval/approve", financeApprovalRequest);

//            // Handle the response as needed
//            if (response.IsSuccessStatusCode)
//            {
//                // Finance approval successful
//            }
//            else
//            {
//                // Handle failure
//            }
//        }
//    }

//    public class PurchaseRequest
//    {
//        // Include necessary properties for a purchase request
//        public UserModel User { get; set; }
//        public string Product { get; set; }
//        public int Amount { get; set; }
//        public bool BuyNowPayLater { get; set; }
//    }
//}
