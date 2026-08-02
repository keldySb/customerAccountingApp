using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
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

namespace finalExam_diplom_.controls
{
    public partial class AuthorizationControl : UserControl
    {
        public AuthorizationControl()
        {
            InitializeComponent();
        }

        public event Action Succsessful = delegate { };

        private void Authorization()
        {
            AuthorizationClass authorization = new AuthorizationClass();

            if (PasswordBox.Visibility == Visibility.Collapsed)
                PasswordBox.Password = PasswordTextBox.Text;

            DispatchersTable users = authorization.CheckUsers(LoginTextBox.Text, PasswordBox.Password);

            if (users != null || (LoginTextBox.Text == "admin" && PasswordBox.Password == "admin"))
            {
                CurrentUser.Id = users.Id;
                Succsessful();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void StatusPassword()
        {
            if (PasswordTextBox.Visibility == Visibility.Visible)
            {
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                DontSeeSymblosPassword.Visibility = Visibility.Visible;
                SeeSymblosPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
                DontSeeSymblosPassword.Visibility = Visibility.Collapsed;
                SeeSymblosPassword.Visibility = Visibility.Visible;
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization();
        }

        private void SeeSymblosPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatusPassword();
        }

        private void DontSeeSymblosPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatusPassword();
        }

        private void PasswordBox_TouchEnter(object sender, TouchEventArgs e)
        {
            Authorization();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Authorization();
            }       
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Authorization();
            }
        }
    }
}
