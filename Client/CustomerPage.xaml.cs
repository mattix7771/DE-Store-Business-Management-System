using ServiceDiscovery;
using InventoryControlService;
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
using PurchaseManagementService;
using SharedModels;

namespace Client
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        ServiceRegistry serviceRegistry;
        UserModel currentUser;
        public CustomerPage(ServiceRegistry serviceRegistryPar, UserModel currentUserPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
            currentUser = currentUserPar;
            ProductListSetup();
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

        private async void btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_productName.Text;
            string productAmount = txt_productAmount.Text;
            bool BNPL = check_BNPL.IsChecked == true? true : false;

            var service = serviceRegistry.GetService<IPurchaseManagementService>();
            await service.MakePurchase(currentUser, productName, int.Parse(productAmount), BNPL);

            productsList.Items.Clear();
            ProductListSetup();
        }
    }
}
