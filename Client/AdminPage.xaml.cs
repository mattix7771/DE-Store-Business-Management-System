using DataAccessLayer;
using InventoryControlService;
using LoyaltyCardService;
using PriceControlService;
using ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserManagementService;

namespace Client
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        ServiceRegistry serviceRegistry;
        public AdminPage(ServiceRegistry serviceRegistryPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
            ProductListSetup();
            LoyaltyUserListSetup();
        }

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
                loyaltyList.Items.Add(new { Username = user.Username, IsAdmin = user.IsAdmin, HaveLoyaltyCard = user.HaveLoyaltyCard });
            }
        }

        private async void btn_create_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_createName.Text;
            string productPrice = txt_createPrice.Text;
            string productStock = txt_createStock.Text;

            var service = serviceRegistry.GetService<IPriceControl>();
            await service.SetProduct(productName, double.Parse(productPrice), int.Parse(productStock));

            lbl_warning.Content = $"Product {productName} added successfully";

            txt_createName.Text = "";
            txt_createPrice.Text = "";
            txt_createStock.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_deleteName.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            await service.DeleteProduct(productName);

            lbl_warning.Content = $"Product {productName} deleted successfully";

            txt_deleteName.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_getProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_getPriceName.Text;

            var service = serviceRegistry.GetService<IPriceControlService>();
            double productPrice = await service.GetProductPrice(productName);

            lbl_warning.Content = $"{productName} price: {productPrice}";

            txt_getPriceName.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_setProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_setPriceName.Text;
            string productPrice = txt_setPricePrice.Text;

            var service = serviceRegistry.GetService<IPriceControlService>();
            await service.SetProductPrice(productName, double.Parse(productPrice));

            lbl_warning.Content = $"Price of {productName} changed successfully";

            txt_setPriceName.Text = "";
            txt_setPricePrice.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_setStock_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_setStockName.Text;
            string amountOrdered = txt_setStockAmount.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            await service.OrderStock(productName, int.Parse(amountOrdered));

            lbl_warning.Content = $"{amountOrdered} units of {productName} have been successfully ordered";

            txt_setStockName.Text = "";
            txt_setStockAmount.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_getStock_Copy_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_getStockName.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            int stock = await service.MonitorStock(productName);

            lbl_warning.Content = $"There are {stock} units of {productName}";

            txt_getStockName.Text = "";

            productsList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_applyLoyalty_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_applyLoyaltyUsername.Text;

            var service = serviceRegistry.GetService<ILoyaltyCardService>();
            await service.ApplyLoyaltyCard(username);

            lbl_loyaltyWarning.Content = $"Loyalty card successfully applied to {username}";

            txt_applyLoyaltyUsername.Text = "";

            loyaltyList.Items.Clear();
            ProductListSetup();
        }

        private async void btn_removeLoyalty_Click(object sender, RoutedEventArgs e)
        {
            string username = txt_removeLoyaltyUsername.Text;

            var service = serviceRegistry.GetService<ILoyaltyCardService>();
            await service.RemoveLoyaltyCard(username);

            lbl_loyaltyWarning.Content = $"Loyalty card successfully removed from {username}";

            txt_removeLoyaltyUsername.Text = "";

            loyaltyList.Items.Clear();
            ProductListSetup();
        }
    }
}
