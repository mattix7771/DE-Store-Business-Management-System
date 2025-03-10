﻿using DataAccessLayer;
using PriceControlService;
using SharedModels;
using ServiceDiscovery;
using InventoryControlService;
using LoyaltyCardService;
using PurchaseManagementService;
using UserManagementService;
using ReportAndAnalysisService;

namespace DE_Store_Business_Management_System
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Welcome message
            Console.WriteLine("Welcome to DE-Store Business Management System");

            // Database Initialisation
            var db = new DataAccessLayer.Database();
            db.DatabaseInitialisation();

            // Creation of instance of the service registry
            IServiceRegistry serviceRegistry = new ServiceRegistry();

            // Initialisation and registration of services
            // Registration of price control service and its actions
            var priceControlServiceImplementation = new PriceControlService.PriceControlService(serviceRegistry);
            serviceRegistry.RegisterService<IPriceControlService>(priceControlServiceImplementation);
            serviceRegistry.RegisterService<IPriceControl>(db);

            // Registration of inventory control service and its actions
            var inventoryControlServiceImplementation = new InventoryControlService.InventoryControlService(serviceRegistry);
            serviceRegistry.RegisterService<IInventoryControlService>(inventoryControlServiceImplementation);
            serviceRegistry.RegisterService<IInventoryControl>(db);

            // Registration of loyalty card service and its actions
            var loyaltyCardServiceImplementation = new LoyaltyCardService.LoyaltyCardService(serviceRegistry);
            serviceRegistry.RegisterService<ILoyaltyCardService>(loyaltyCardServiceImplementation);
            serviceRegistry.RegisterService<ILoyaltyCard>(db);

            // Registration of purchase management service and its actions
            var purchaseManagementServiceImplementation = new PurchaseManagementService.PurchaseManagementService(serviceRegistry);
            serviceRegistry.RegisterService<IPurchaseManagementService>(purchaseManagementServiceImplementation);
            serviceRegistry.RegisterService<IPurchaseManagement>(db);

            // Registration of user management service and its actions
            var userManagementServiceImplementation = new UserManagementService.UserManagementService(serviceRegistry);
            serviceRegistry.RegisterService<IUserManagementService>(userManagementServiceImplementation);
            serviceRegistry.RegisterService<IUserManagement>(db);

            // Registration of report and analysis service and its actions
            var reportAndAnalysisServiceImplementation = new ReportAndAnalysisService.ReportAndAnalysisService(serviceRegistry);
            serviceRegistry.RegisterService<IReportAndAnalysisService>(reportAndAnalysisServiceImplementation);
            serviceRegistry.RegisterService<IReportAndAnalysis>(db);

            

            // Now we can retrieve all the services needed from the service registry
            var priceControlService = serviceRegistry.GetService<IPriceControlService>();
            var inventoryControlService = serviceRegistry.GetService<IInventoryControlService>();
            var loyaltyCardService = serviceRegistry.GetService<ILoyaltyCardService>();
            var purchaseManagementService = serviceRegistry.GetService<IPurchaseManagementService>();
            var userManagementService = serviceRegistry.GetService<IUserManagementService>();
            var reportAndAnalysisService = serviceRegistry.GetService<IReportAndAnalysisService>();











            //var serviceRegistry = new ServiceRegistry();
            //serviceRegistry.RegisterService<IPriceControl>(db);

            ////var priceControlService = new PriceControlService(serviceRegistry);

            //// Initialisation of services
            //var priceControlService1 = new PriceControlService.PriceControlService(serviceRegistry);
            //InventoryControlService.InventoryControlService inventoryControlService = new InventoryControlService.InventoryControlService();
            //LoyaltyCardService.LoyaltyCardService loyaltyCardService = new LoyaltyCardService.LoyaltyCardService();
            //PurchaseManagementService.PurchaseManagementService purchaseManagementService = new PurchaseManagementService.PurchaseManagementService();
            //UserManagementService.UserManagementService userManagementService = new UserManagementService.UserManagementService();
            //ReportAndAnalysisService.ReportAndAnalysisService reportAndAnalysisService = new ReportAndAnalysisService.ReportAndAnalysisService();






















            // Current user
            UserModel currentUser = new UserModel();

            // User selection
            List<UserModel> users = await userManagementService.GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No user found, please create a new user");
                Console.WriteLine("Enter a username: ");
                string username = Console.ReadLine();
                Console.WriteLine("Enter a password: ");
                string password = Console.ReadLine();
                Console.WriteLine("Are you an admin? (y/n)");
                string isadmin = Console.ReadLine();
                await userManagementService.CreateUser(isadmin == "y" ? true : false, username, password, false);

                currentUser = await userManagementService.GetUser(username);
            }
            else
            {
                Console.WriteLine("Please select user to use");
                for (int i = 0; i < users.Count; i++)
                {
                    UserModel entry = users[i];
                    Console.WriteLine($"{i + 1}. {entry.Username}");
                }

                while (currentUser.Username == null)
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
                        { Console.WriteLine($"{productName} costs £{await priceControlService.GetProductPrice(productName)}"); }
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
                        if (currentUser.IsAdmin)
                        {
                            try
                            {
                                List<ProductModel> products = await inventoryControlService.GenerateWarnings();

                                if (products.Count > 0)
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
        }
    }
}
