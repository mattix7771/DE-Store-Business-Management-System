using ServiceDiscovery;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UserManagementService;

namespace Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        ServiceRegistry serviceRegistry;

        public Login(ServiceRegistry serviceRegistryPar)
        {
            InitializeComponent();
            serviceRegistry = serviceRegistryPar;
        }

        private async void btn_loginSubmit(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Text;

            var userManagementService = serviceRegistry.GetService<IUserManagementService>();

            try
            {
                var currentUser = await userManagementService.GetUser(username);

                if (currentUser == null)
                {
                    lbl_loginError.Content = "Error - user not found";
                    currentUser = null;
                }
                else
                {
                    MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                    if (currentUser.IsAdmin)
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
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                lbl_loginError.Content = $"Error: {ex.Message}";
            }
        }

    }
}
