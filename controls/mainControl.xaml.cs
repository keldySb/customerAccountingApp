using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using Npgsql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace finalExam_diplom_.controls
{
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
        }

        private void MainTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mainTextBox.Text == "Поиск")
                mainTextBox.Text = "";
        }

        private void MainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mainTextBox.Text))
                mainTextBox.Text = "Поиск"; 
        }

        private void ShowDataGridnformations()
        {
            GetClientsRequestsTable client = new GetClientsRequestsTable();

            List<ClientsRequestTable> data = client.GetClientsRequests(mainTextBox.Text, Convert.ToInt32(mainComboBox.SelectedValue));

            mainDataGrid.ItemsSource = data;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetStatuses status = new GetStatuses();

            mainComboBox.ItemsSource = status.GetStatus();
            mainComboBox.SelectedValuePath = "Id";
            mainComboBox.DisplayMemberPath = "Name";

            ShowDataGridnformations();
        }

        private void mainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowDataGridnformations();
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clientData = mainDataGrid.SelectedItem as ClientsRequestTable;

            clientsChangeDataControl control = new clientsChangeDataControl(clientData);
            addAndChangeForm form = new addAndChangeForm(control);
            form.ShowDialog();
        }

        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDataGridnformations();
        }
    }
}
