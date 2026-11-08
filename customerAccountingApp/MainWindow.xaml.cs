using finalExam_diplom_.controls;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckStatus()
        {
            AuthorizationControl auth = new AuthorizationControl();
            authControl.Content = auth;
            authControl.Visibility = Visibility.Visible;
            MainMenu.Visibility = Visibility.Collapsed;
            mainControl.Visibility = Visibility.Collapsed;
            headerTextBox.Text = "Авторизация";
            auth.Succsessful += Sucsessful;
        }

        private void Sucsessful()
        {
            authControl.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
            mainControl.Visibility = Visibility.Visible;

            MainButton_Click(mainButton, new RoutedEventArgs());

        }

        private void CheckStateButton(object sender)
        {
            Button[] buttons = { mainButton, reportButton };

            Button? blockButton = sender as Button;

            foreach (Button button in buttons)
            {
                button.IsEnabled = true;
            }

            if (blockButton != null)
                blockButton.IsEnabled = false;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainControl mc = new MainControl(null);
            useControl.Content = mc;
            headerTextBox.Text = "Заявки клиентов";

            CheckStateButton(mainButton);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            reportControl rc = new reportControl();
            useControl.Content = rc;
            headerTextBox.Text = "Отчетность";

            CheckStateButton(reportButton);
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            CheckStatus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckStatus();
        }
    }
}