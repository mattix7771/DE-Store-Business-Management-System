using DataAccessLayer;
using InventoryControlService;
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
            ListSetup();
        }

        public async void ListSetup()
        {
            var service = serviceRegistry.GetService<IInventoryControlService>();
            var products = await service.GetAllProducts();

            // Create a ListView
            var listView = new ListView();

            // Create GridView to define columns
            var gridView = new GridView();

            // Define columns
            gridView.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("Name"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Price", DisplayMemberBinding = new Binding("Price"), Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Stock", DisplayMemberBinding = new Binding("Stock"), Width = 100 });

            // Set GridView as ListView view
            listView.View = gridView;

            // Add products to the ListView
            foreach (var product in products)
            {
                listView.Items.Add(new { Name = product.Name, Price = product.Price, Stock = product.Stock });
            }

            // Add the ListView to your UI
            grid.Children.Add(listView);
        }

        private async void btn_create_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_createName.Text;
            string productPrice = txt_createPrice.Text;
            string productStock = txt_createStock.Text;

            var service = serviceRegistry.GetService<IPriceControl>();
            await service.SetProduct(productName, double.Parse(productPrice), int.Parse(productStock));

            lbl_warning.Content = $"Product {productName} added successfully";

            productName = "";
            productPrice = "";
            productStock = "";
        }

        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_deleteName.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            await service.DeleteProduct(productName);

            lbl_warning.Content = $"Product {productName} deleted successfully";

            productName = "";
        }

        private async void btn_getProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_getPriceName.Text;

            var service = serviceRegistry.GetService<IPriceControlService>();
            double productPrice = await service.GetProductPrice(productName);

            lbl_warning.Content = $"{productName} price: {productPrice}";

            productName = "";
        }

        private async void btn_setProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_setPriceName.Text;
            string productPrice = txt_setPricePrice.Text;

            var service = serviceRegistry.GetService<IPriceControlService>();
            await service.SetProductPrice(productName, double.Parse(productPrice));

            lbl_warning.Content = $"Price of {productName} changed successfully";

            productName = "";
            productPrice = "";
        }

        private async void btn_setStock_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_setStockName.Text;
            string amountOrdered = txt_setStockAmount.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            await service.OrderStock(productName, int.Parse(amountOrdered));

            lbl_warning.Content = $"{amountOrdered} units of {productName} have been successfully ordered";

            productName = "";
            amountOrdered = "";
        }

        private async void btn_getStock_Copy_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_getStockName.Text;

            var service = serviceRegistry.GetService<IInventoryControlService>();
            int stock = await service.MonitorStock(productName);

            lbl_warning.Content = $"There are {stock} units of {productName}";

            productName = "";
        }

        private async void btn_applyLoyalty_Click(object sender, RoutedEventArgs e)
        {
            edwd
        }
    }
}
