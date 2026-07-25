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
    /// <summary>
    /// Логика взаимодействия для AuthorizationControl.xaml
    /// </summary>
    public partial class AuthorizationControl : UserControl
    {
        public AuthorizationControl()
        {
            InitializeComponent();
        }

        public event Action Succsessful = delegate { };

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationClass authorization = new AuthorizationClass();

            List<DispatchersTable> users = authorization.CheckUsers(LoginTextBox.Text, PasswordTextBox.Text);

            if (users.Count > 0)
            {
                Succsessful();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
