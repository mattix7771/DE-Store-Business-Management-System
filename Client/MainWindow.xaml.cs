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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceRegistry serviceRegistry = new ServiceRegistry();

        public MainWindow()
        {
            InitializeComponent();
            ServiceManagement.ServiceManagement serviceManagement = new ServiceManagement.ServiceManagement();
            serviceManagement.ServiceInitialisation(serviceRegistry);
        }

        /// <summary>
        /// Redirect to login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login(serviceRegistry);
            this.Content = loginPage;
        }

        /// <summary>
        /// Redirect to registration page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registrationPage = new Registration(serviceRegistry);
            this.Content = registrationPage;
        }
    }
}
