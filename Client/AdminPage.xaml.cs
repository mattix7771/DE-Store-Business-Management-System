using DataAccessLayer;
using InventoryControlService;
using LoyaltyCardService;
using PriceControlService;
using ReportAndAnalysisService;
using ServiceDiscovery;
using SharedModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UserManagementService;

namespace Client
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        /// <summary>
        /// Setup of registry, lists, and current user
        /// </summary>
        ServiceRegistry serviceRegistry;
        UserModel currentUser;
        public AdminPage(ServiceRegistry serviceRegistryPar, UserModel currentUserPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
            currentUser = currentUserPar;
            ProductListSetup();
            ProductListLowStockSetup();
            LoyaltyUserListSetup();
            AllUsersListSetup();
            AllTransactionsListSetup();
            UserTransactionListSetup();
        }

        /// <summary>
        /// Setup for products list
        /// </summary>
        public async void ProductListSetup()
        {
            var service = serviceRegistry.GetService<IInventoryControlService>();
            var products = await service.GetAllProducts();

            // Create GridView to define columns
            var gridView = new GridView();

            // Define columns
            gridView.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("Name"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Price", DisplayMemberBinding = new Binding("Price"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Stock", DisplayMemberBinding = new Binding("Stock"), Width = 100 });

            // Set GridView as loyaltyList view
            productsList.View = gridView;

            // Add products to the loyaltyList
            foreach (var product in products)
            {
                productsList.Items.Add(new { Name = product.Name, Price = product.Price, Stock = product.Stock });
            }
        }

        /// <summary>
        /// Setup for list of products on low stock
        /// </summary>
        public async void ProductListLowStockSetup()
        {
            var service = serviceRegistry.GetService<IInventoryControlService>();
            var products = await service.GenerateWarnings();

            // Create GridView to define columns
            var gridView = new GridView();

            // Define columns
            gridView.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("Name"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Price", DisplayMemberBinding = new Binding("Price"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Stock", DisplayMemberBinding = new Binding("Stock"), Width = 100 });

            // Set GridView as loyaltyList view
            productsListLowStock.View = gridView;

            // Add products to the loyaltyList
            foreach (var product in products)
            {
                productsListLowStock.Items.Add(new { Name = product.Name, Price = product.Price, Stock = product.Stock });
            }
        }

        /// <summary>
        /// Setup for user list
        /// </summary>
        private async void LoyaltyUserListSetup()
        {
            var userService = serviceRegistry.GetService<IUserManagementService>();
            var users = await userService.GetAllUsers();

            var userGridView = new GridView();

            userGridView.Columns.Add(new GridViewColumn { Header = "Username", DisplayMemberBinding = new Binding("Username") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Is Admin", DisplayMemberBinding = new Binding("IsAdmin") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Has Loyalty Card", DisplayMemberBinding = new Binding("HaveLoyaltyCard") });

            loyaltyList.View = userGridView;

            foreach (var user in users)
            {
                if (user.HaveLoyaltyCard)
                    loyaltyList.Items.Add(new { Username = user.Username, IsAdmin = user.IsAdmin, HaveLoyaltyCard = user.HaveLoyaltyCard });
            }
        }

        /// <summary>
        /// Setup for all users list
        /// </summary>
        private async void AllUsersListSetup()
        {
            var userService = serviceRegistry.GetService<IUserManagementService>();
            var users = await userService.GetAllUsers();

            var userGridView = new GridView();

            userGridView.Columns.Add(new GridViewColumn { Header = "Username", DisplayMemberBinding = new Binding("Username") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Is Admin", DisplayMemberBinding = new Binding("IsAdmin") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Has Loyalty Card", DisplayMemberBinding = new Binding("HaveLoyaltyCard") });

            userList.View = userGridView;

            foreach (var user in users)
            {
                userList.Items.Add(new { Username = user.Username, IsAdmin = user.IsAdmin, HaveLoyaltyCard = user.HaveLoyaltyCard });
            }
        }

        /// <summary>
        /// Setup for all transactions list
        /// </summary>
        private async void AllTransactionsListSetup()
        {
            var service = serviceRegistry.GetService<IReportAndAnalysisService>();
            var transactions = await service.StoreAnalysis();

            var userGridView = new GridView();

            userGridView.Columns.Add(new GridViewColumn { Header = "User", DisplayMemberBinding = new Binding("User.Username") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Product", DisplayMemberBinding = new Binding("Product") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Amount", DisplayMemberBinding = new Binding("Amount") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Buy Now Pay Later", DisplayMemberBinding = new Binding("BuyNowPayLater") });

            transactionList.View = userGridView;

            foreach (var transaction in transactions)
            {
                transactionList.Items.Add(new { Username = transaction.User.Username, Product = transaction.Product, Amount = transaction.Amount, BNPL = transaction.BuyNowPayLater });
            }
        }

        /// <summary>
        /// Setup for transactions list
        /// </summary>
        private async void UserTransactionListSetup()
        {
            var service = serviceRegistry.GetService<IReportAndAnalysisService>();
            var transactions = await service.StoreAnalysis();

            var userGridView = new GridView();

            userGridView.Columns.Add(new GridViewColumn { Header = "User", DisplayMemberBinding = new Binding("User.Username") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Product", DisplayMemberBinding = new Binding("Product") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Amount", DisplayMemberBinding = new Binding("Amount") });
            userGridView.Columns.Add(new GridViewColumn { Header = "Buy Now Pay Later", DisplayMemberBinding = new Binding("BuyNowPayLater") });

            userTransactionList.View = userGridView;

            foreach (var transaction in transactions)
            {
                if (transaction.User.Username == currentUser.Username)
                    userTransactionList.Items.Add(new { Username = transaction.User.Username, Product = transaction.Product, Amount = transaction.Amount, BNPL = transaction.BuyNowPayLater });
            }
        }

        /// <summary>
        /// Logic to verify most system funcitonality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (radio_Create.IsChecked == true)
            {
                string productName = txt_ProductName.Text;
                string productPrice = txt_createPrice.Text;
                string productStock = txt_createStock.Text;

                try
                {
                    var service = serviceRegistry.GetService<IPriceControl>();
                    await service.SetProduct(productName, double.Parse(productPrice), int.Parse(productStock));
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Couldn't add product";
                    return;
                }

                lbl_warning.Content = $"Product {productName} added successfully";

                txt_ProductName.Text = "";
                txt_createPrice.Text = "";
                txt_createStock.Text = "";
            }
            else if (radio_Delete.IsChecked == true)
            {
                string productName = txt_ProductName.Text;

                try
                {
                    var service = serviceRegistry.GetService<IInventoryControlService>();
                    await service.DeleteProduct(productName);
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Product not found";
                    return;
                }

                lbl_warning.Content = $"Product {productName} deleted successfully";

                txt_ProductName.Text = "";
            }
            else if (radio_SetPrice.IsChecked == true)
            {
                string productName = txt_ProductName.Text;
                string productPrice = txt_setPricePrice.Text;

                try
                {
                    var service = serviceRegistry.GetService<IPriceControlService>();
                    await service.SetProductPrice(productName, double.Parse(productPrice));
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Product not found";
                    return;
                }

                lbl_warning.Content = $"Price of {productName} changed successfully";

                txt_ProductName.Text = "";
                txt_setPricePrice.Text = "";
            }
            else if (radio_GetPrice.IsChecked == true)
            {
                string productName = txt_ProductName.Text;
                double productPrice;

                try
                {
                    var service = serviceRegistry.GetService<IPriceControlService>();
                    productPrice = await service.GetProductPrice(productName);
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Product not found";
                    return;
                }

                lbl_warning.Content = $"{productName} price: {productPrice}";

                txt_ProductName.Text = "";
            }
            else if (radio_BuyStock.IsChecked == true)
            {
                string productName = txt_ProductName.Text;
                string amountOrdered = txt_setStockAmount.Text;

                try
                {
                    var service = serviceRegistry.GetService<IInventoryControlService>();
                    await service.OrderStock(productName, int.Parse(amountOrdered));
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Product not found";
                    return;
                }

                lbl_warning.Content = $"{amountOrdered} units of {productName} have been successfully ordered";

                txt_ProductName.Text = "";
                txt_setStockAmount.Text = "";
            }
            else if (radio_ViewStock.IsChecked == true)
            {
                string productName = txt_ProductName.Text;
                int stock;

                try
                {
                    var service = serviceRegistry.GetService<IInventoryControlService>();
                    stock = await service.MonitorStock(productName);
                }
                catch (Exception ex)
                {
                    lbl_warning.Content = "Product not found";
                    return;
                }

                lbl_warning.Content = $"There are {stock} units of {productName}";

                txt_ProductName.Text = "";
            }

            // Update lists
            productsList.Items.Clear();
            ProductListSetup();
            productsListLowStock.Items.Clear();
            ProductListLowStockSetup();
        }

        /// <summary>
        /// Apply loyalty card to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_applyLoyalty_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_applyLoyaltyUsername.Text;

            try
            {
                var service = serviceRegistry.GetService<ILoyaltyCardService>();
                await service.ApplyLoyaltyCard(username);
            }
            catch (Exception ex)
            {
                lbl_loyaltyWarning.Content = "User not found";
                return;
            }

            lbl_loyaltyWarning.Content = $"Loyalty card successfully applied to {username}";

            txt_applyLoyaltyUsername.Text = "";

            loyaltyList.Items.Clear();
            LoyaltyUserListSetup();
        }

        /// <summary>
        /// Remove loyalty card from user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_removeLoyalty_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_removeLoyaltyUsername.Text;

            try
            {
                var service = serviceRegistry.GetService<ILoyaltyCardService>();
                await service.RemoveLoyaltyCard(username);
            }
            catch (Exception ex)
            {
                lbl_loyaltyWarning.Content = "User not found";
                return;
            }

            lbl_loyaltyWarning.Content = $"Loyalty card successfully removed from {username}";

            txt_removeLoyaltyUsername.Text = "";

            loyaltyList.Items.Clear();
            LoyaltyUserListSetup();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_deleteUsername_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_deleteUsername.Text;

            try
            {
                var service = serviceRegistry.GetService<IUserManagementService>();
                await service.DeleteUser(username);
            }
            catch (Exception ex)
            {
                lbl_userWarning.Content = "User not found";
                return;
            }

            lbl_userWarning.Content = $"{username} successfully deleted";

            txt_deleteUsername.Text = "";

            userList.Items.Clear();
            AllUsersListSetup();
        }

        // Visibility Settings for all the different forms
        private void radio_Create_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "Create Product";
            lbl_ProductName.Visibility = Visibility.Visible;
            lbl_ProductPrice.Visibility = Visibility.Visible;
            lbl_ProductStock.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            txt_createPrice.Visibility = Visibility.Visible;
            txt_createStock.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_Create_Unchecked(object sender, RoutedEventArgs e)
        {
            lbl_ProductPrice.Visibility = Visibility.Hidden;
            lbl_ProductStock.Visibility = Visibility.Hidden;
            txt_createPrice.Visibility = Visibility.Hidden;
            txt_createStock.Visibility = Visibility.Hidden;
        }

        private void radio_Delete_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "Delete Product";
            lbl_ProductName.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_Delete_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void radio_SetPrice_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "Change Product Price";
            lbl_NewPrice.Visibility = Visibility.Visible;
            txt_setPricePrice.Visibility = Visibility.Visible;
            lbl_ProductName.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_SetPrice_Unchecked(object sender, RoutedEventArgs e)
        {
            lbl_NewPrice.Visibility = Visibility.Hidden;
            txt_setPricePrice.Visibility = Visibility.Hidden;
        }

        private void radio_GetPrice_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "Get Product Price";
            lbl_ProductName.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_GetPrice_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void radio_BuyStock_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "Buy Product Stock";
            lbl_stockOrdered.Visibility = Visibility.Visible;
            txt_setStockAmount.Visibility = Visibility.Visible;
            lbl_ProductName.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_BuyStock_Unchecked(object sender, RoutedEventArgs e)
        {
            lbl_stockOrdered.Visibility = Visibility.Hidden;
            txt_setStockAmount.Visibility = Visibility.Hidden;
        }

        private void radio_ViewStock_Checked(object sender, RoutedEventArgs e)
        {
            lbl_Title.Content = "View Product Stock";
            lbl_ProductName.Visibility = Visibility.Visible;
            txt_ProductName.Visibility = Visibility.Visible;
            btn_Submit.Visibility = Visibility.Visible;
        }

        private void radio_ViewStock_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
