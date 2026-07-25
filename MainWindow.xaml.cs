using finalExam_diplom_.controls;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
            headerTextBox.Text = "Авторизация";
            auth.Succsessful += Sucsessful;
        }

        private void Sucsessful()
        {
            authControl.Visibility = Visibility.Collapsed;
        }

        private void SetTask()
        {
            setBorder.Visibility = Visibility.Visible;
            Splitter.Visibility = Visibility.Visible;
            MainControl mc = new MainControl();
            setTaskOrEmpControl.Content = mc;

            mc.statusBorder.Visibility = Visibility.Collapsed;
            mc.Id.Visibility = Visibility.Collapsed;
            mc.Fio.Visibility = Visibility.Collapsed;
            mc.Status.Visibility = Visibility.Collapsed;
            mc.Created_time.Visibility = Visibility.Collapsed;
        }

        private void HideObjects()
        {
            setBorder.Visibility = Visibility.Collapsed;
            Splitter.Visibility = Visibility.Collapsed;
            mainGrid.ColumnDefinitions[3].Width = GridLength.Auto;
        }

        private void CheckStateButton(object sender)
        {
            Button[] buttons = { mainButton, employeeButton, reportButton };

            Button? blockButton = sender as Button;

            foreach (Button button in buttons)
            {
                button.IsEnabled = true;
            }

            if (blockButton != null)
                blockButton.IsEnabled = false;

            HideObjects();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            employeeControl ec = new employeeControl();
            useControl.Content = ec;
            headerTextBox.Text = "Список сотрудников";

            ec.setTask += SetTask;

            CheckStateButton(employeeButton);
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainControl mc = new MainControl();
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

        private void CloseUCButton_Click(object sender, RoutedEventArgs e)
        {
            HideObjects();
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