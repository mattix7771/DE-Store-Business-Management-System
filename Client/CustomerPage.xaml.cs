using ServiceDiscovery;
using InventoryControlService;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PurchaseManagementService;
using SharedModels;
using System.Threading;
using System.Xml.Linq;

namespace Client
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        /// <summary>
        /// Constructor and list setup
        /// </summary>
        ServiceRegistry serviceRegistry;
        UserModel currentUser;
        public CustomerPage(ServiceRegistry serviceRegistryPar, UserModel currentUserPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
            currentUser = currentUserPar;
            ProductListSetup();

            if (currentUser.HaveLoyaltyCard)
                lbl_products.Content += " (Sale!)";
        }

        /// <summary>
        /// Setup list to show avaliable products
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
                if (!currentUser.HaveLoyaltyCard)
                    productsList.Items.Add(new { Name = product.Name, Price = product.Price, Stock = product.Stock });
                else
                    productsList.Items.Add(new { Name = product.Name, Price = product.Price - product.Price/2, Stock = product.Stock });
            }
        }

        /// <summary>
        /// Logic to handle cutomer purchase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_productName.Text;
            string productAmount = txt_productAmount.Text;
            bool BNPL = check_BNPL.IsChecked == true? true : false;

            try
            {
                var service = serviceRegistry.GetService<IPurchaseManagementService>();
                var inventory = serviceRegistry.GetService<IInventoryControlService>();

                var products = await inventory.GetAllProducts();
                var isValid = products.Any(product => product.Name == productName);

                lbl_warning.Content = isValid;

                if (isValid && products.FirstOrDefault(product => product.Name == productName).Stock >= int.Parse(productAmount))
                {
                    if (BNPL)
                    {
                        lbl_warning.Content = "Approving payment though Enabling portal";
                        Thread.Sleep(2000);
                    }
                    await service.MakePurchase(currentUser, productName, int.Parse(productAmount), BNPL);
                    lbl_warning.Content = "Purchase successful";
                }
                else
                    lbl_warning.Content = "Cannot purchase this product";
            }
            catch (Exception ex)
            {
                lbl_warning.Content = "Invalid input";
            }

            productsList.Items.Clear();
            ProductListSetup();
        }
    }
}
