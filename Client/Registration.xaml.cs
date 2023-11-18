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

        private void btn_regSubmit(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Text;
            bool isAdmin = check_isAdmin.IsChecked.Value;

            var userManagementService = serviceRegistry.GetService<IUserManagementService>();
            userManagementService.CreateUser(isAdmin, username, password, false);

            var currentUser = userManagementService.GetUser(username);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (currentUser.Result.IsAdmin)
            {
                var adminPage = new AdminPage(serviceRegistry);
                mw.Content = adminPage;
            }
            else
            {
                var customerPage = new CustomerPage(serviceRegistry);
                mw.Content = customerPage;
            }
        }
    }
}
