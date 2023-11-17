using DataAccessLayer;
using PriceControlService;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace DE_Store_Business_Management_System
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Welcome message
            Console.WriteLine("Welcome to DE-Store Business Management System");

            // Database Initialisation
            DataAccessLayer.Database db = new DataAccessLayer.Database();
            db.DatabaseInitialisation();

            // Initialisation of services
            PriceControlService.PriceControlService priceControlService = new PriceControlService.PriceControlService();
            InventoryControlService.InventoryControlService inventoryControlService = new InventoryControlService.InventoryControlService();
            LoyaltyCardService.LoyaltyCardService loyaltyCardService = new LoyaltyCardService.LoyaltyCardService();
            PurchaseManagementService.PurchaseManagementService purchaseManagementService = new PurchaseManagementService.PurchaseManagementService();
            UserManagementService.UserManagementService userManagementService = new UserManagementService.UserManagementService();
            ReportAndAnalysisService.ReportAndAnalysisService reportAndAnalysisService = new ReportAndAnalysisService.ReportAndAnalysisService();

            // Current user
            UserModel currentUser = new UserModel();
            
            // User selection
            List<UserModel> users = await userManagementService.GetAllUsers();
            if (users.Count == 0)
                Console.WriteLine("No user found, please create a new user");
            else
            {
                Console.WriteLine("Please select user to use");
                for (int i = 0; i < users.Count; i++)
                {
                    UserModel entry = users[i];
                    Console.WriteLine($"{i + 1}. {entry.Username}");
                }

                while(currentUser.Username == null)
                {
                    Console.Write("Enter the number of the user you want to select: ");
                    if (int.TryParse(Console.ReadLine(), out int index))
                    {
                        if (index > 0 && index <= users.Count)
                        {
                            currentUser = users[index - 1];
                            Console.WriteLine($"You selected user: {currentUser.Username}");
                        }
                        else
                            Console.WriteLine("Invalid selection");
                    }
                    else
                        Console.WriteLine("Invalid input");
                }
            }

            // Main loop to call all the different services and their respective functionalities
            while (true)
            {
                Console.WriteLine("\n\nHere's a list of actions you can take");
                Console.WriteLine("1. Get Product Price");
                Console.WriteLine("2. Set Product Price");
                Console.WriteLine("3. Order Stock");
                Console.WriteLine("4. Monitor Stock");
                Console.WriteLine("5. Generate Warnings");
                Console.WriteLine("6. Apply Loyalty Card");
                Console.WriteLine("7. Revoke Loyalty Card");
                Console.WriteLine("8. Get All Loyalty Card Holders");
                Console.WriteLine("9. Make a purchase");
                Console.WriteLine("10. Get user purchases");
                Console.WriteLine("11. View Store Performace");

                string input = Console.ReadLine();
                switch (int.Parse(input))
                {
                    case 1:
                        Console.Write("Enter a product name: ");
                        string productName = Console.ReadLine();
                        try
                        { Console.WriteLine($"{productName} costs £{priceControlService.GetProductPrice(productName)}"); }
                        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        break;

                    case 2:
                        if (currentUser.IsAdmin)
                        {
                            Console.Write("Enter a product name: ");
                            string productName2 = Console.ReadLine();
                            Console.Write("Enter the new product price: ");
                            string productPrice = Console.ReadLine();
                            try
                            {
                                await priceControlService.SetProductPrice(productName2, double.Parse(productPrice));
                                Console.WriteLine("Price changed successfully");
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 3:
                        if (currentUser.IsAdmin)
                        {
                            Console.Write("Enter a product name: ");
                            string productName3 = Console.ReadLine();
                            Console.Write($"Enter amount of {productName3} bought: ");
                            string stock = Console.ReadLine();
                            try 
                            { 
                                await inventoryControlService.OrderStock(productName3, int.Parse(stock));
                                Console.WriteLine($"{stock} {productName3} have been ordered");
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 4:
                        Console.Write("Enter a product name: ");
                        string productName4 = Console.ReadLine();
                        try
                        { Console.WriteLine($"There are {inventoryControlService.MonitorStock(productName4)} units of {productName4}"); }
                        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        break;

                    case 5:
                        if(currentUser.IsAdmin)
                        {
                            try 
                            { 
                                List<ProductModel> products = await inventoryControlService.GenerateWarnings();

                                if(products.Count > 0)
                                {
                                    Console.WriteLine("Items with low stock: ");
                                    foreach (ProductModel product in products)
                                    {
                                        Console.WriteLine(product.Name);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("There are no items low on stock");
                                }
                                
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 6:
                        if (currentUser.IsAdmin)
                        {
                            Console.Write("Enter a username: ");
                            string username = Console.ReadLine();

                            try
                            {
                                await loyaltyCardService.ApplyLoyaltyCard(username);
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

                            Console.WriteLine($"Successfully applied loyalty card to {username}");
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 7:
                        if (currentUser.IsAdmin)
                        {
                            Console.Write("Enter a username: ");
                            string username = Console.ReadLine();

                            try
                            {
                                await loyaltyCardService.RemoveLoyaltyCard(username);
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

                            Console.WriteLine($"Successfully revoked loyalty from {username}");
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 8:
                        if (currentUser.IsAdmin)
                        {
                            List<UserModel> loyaltyCardHolders = new List<UserModel>();
                            try
                            {
                                loyaltyCardHolders = await loyaltyCardService.LoyaltyCardHolders();
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

                            if (loyaltyCardHolders.Count > 0)
                            {
                                Console.WriteLine("Users with a loyalty card:");
                                foreach (UserModel possibleLoyaltyCardHolder in loyaltyCardHolders)
                                    Console.WriteLine(possibleLoyaltyCardHolder.Username);
                            }
                            else
                            {
                                Console.WriteLine("There are no users with an active loyalty card");
                            }
                        }
                        else { Console.WriteLine("Only a system admin can take this action!"); }
                        break;

                    case 9:
                        Console.WriteLine("What product would you like to purchase?");

                        try
                        {
                            List<ProductModel> products9 = await inventoryControlService.GetAllProducts();
                            for (int i = 0; i < products9.Count; i++)
                                Console.WriteLine(products9[i].Name);
                            string productChoice = Console.ReadLine();

                            Console.WriteLine("How many units would you like to purchase?");
                            int purchaseAmount = int.Parse(Console.ReadLine());

                            Console.WriteLine("Would you like to buy now and pay later? (y/n)");
                            bool buyNowPayLater = Console.ReadLine() == "y" ? true : false;

                            await purchaseManagementService.MakePurchase(currentUser, productChoice, purchaseAmount, buyNowPayLater);
                        }
                        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        break;

                    case 10:
                        if (currentUser.IsAdmin)
                        {
                            Console.Write("Enter the number of a user: ");
                            string user10 = Console.ReadLine();

                            try
                            {
                                UserModel findUser = await userManagementService.GetUser(user10);
                                List<TransactionModel> userTransactions = await purchaseManagementService.GetUserPurchases(findUser);

                                if (userTransactions.Count < 1)
                                    Console.WriteLine("This user has no past transactions");
                                else
                                {
                                    foreach (TransactionModel transaction in userTransactions)
                                        Console.WriteLine($"username: {findUser.Username}, product bought: {transaction.Product}, amount bought: {transaction.Amount}, buyNowPayLater: {transaction.BuyNowPayLater}");
                                }

                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        }
                        break;

                    case 11:
                        if (currentUser.IsAdmin)
                        {
                            try
                            {
                                List<TransactionModel> transactions = await reportAndAnalysisService.StoreAnalysis();
                                foreach (TransactionModel transaction in transactions)
                                    Console.WriteLine($"username: {transaction.User.Username}, product bought: {transaction.Product}, amount bought: {transaction.Amount}, buyNowPayLater: {transaction.BuyNowPayLater}");
                            }
                            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                        }
                        break;

                    default:
                        Console.WriteLine("Inavid option");
                        break;
                }
            }

            
            priceControlService.SetProductPrice("banana", 1.3);

            //user.CreateUser(true, "mattix7771", "12345678", true);
            






            //x.SetProduct("banana", 0.25, 600);
            //x.SetProduct("apple", 0.31, 550);
            //x.SetProduct("orange", 0.46, 330);
            //x.SetProduct("mango", 1.2, 52);
            //x.SetProduct("kiwi", 1.7, 17);

            //priceControlService.SetProductPrice("apple", 1.5);


            //switch (input)
            //{
            //    case ""
            //}

            //Task<ProductModel> model = x.GetProduct("Nicole", 0.0,0, "name");
            //Console.WriteLine(model.Result.Name);
            //Console.WriteLine(model.Result.Stock);
            //Console.WriteLine(model.Result.Price);
        }
    }
}
