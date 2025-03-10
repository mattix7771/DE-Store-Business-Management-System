﻿using ServiceDiscovery;
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

        /// <summary>
        /// Logic to verify login credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                else if (currentUser.Password != password)
                {
                    lbl_loginError.Content = "Error - invalid credentials";
                }   
                else
                {
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
            catch (Exception ex)
            {
                lbl_loginError.Content = $"Error: {ex.Message}";
            }
        }

    }
}
