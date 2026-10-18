using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace finalExam_diplom_.controls
{
    public partial class AuthorizationControl : UserControl
    {
        public AuthorizationControl()
        {
            InitializeComponent();
        }

        public event Action Succsessful = delegate { };

        private async void Authorization()
        {
            AuthClient authorization = new AuthClient();

            if (PasswordBox.Visibility == Visibility.Collapsed)
                PasswordBox.Password = PasswordTextBox.Text;

            var auth = new AuthClient();

            try
            {
                DispatchersTable? users = await auth.LoginAsync(
                            LoginTextBox.Text,
                            PasswordBox.Password);
                if (users != null)
                {
                    CurrentUser.Id = users.Id;
                    Succsessful();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            } catch (HttpRequestException)
            {
                MessageBox.Show("Не удалось подключиться к серверу авторизации");
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
