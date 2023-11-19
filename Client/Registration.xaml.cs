using ServiceDiscovery;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UserManagementService;

namespace Client
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        ServiceRegistry serviceRegistry;

        public Registration(ServiceRegistry serviceRegistryPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
        }

        private async void btn_regSubmit(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Text;
            bool isAdmin = check_isAdmin.IsChecked == true? true: false;

            var service = serviceRegistry.GetService<IUserManagementService>();
            await service.CreateUser(isAdmin, username, password, false);

            var currentUser = await service.GetUser(username);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (currentUser.IsAdmin)
            {
                var adminPage = new AdminPage(serviceRegistry, currentUser);
                mw.Content = adminPage;
            }
            else
            {
                var customerPage = new CustomerPage(serviceRegistry, currentUser);
                mw.Content = customerPage;
            }
        }
    }
}
