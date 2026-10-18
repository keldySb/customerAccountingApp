using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using Npgsql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace finalExam_diplom_.controls
{
    public partial class MainControl : UserControl
    {
        public Action? _HideObjects;
        public Action? RequesInfo;
        private DispatcherTimer timer;

        public MainControl(Action hideObjects)
        {
            InitializeComponent();
            _HideObjects = hideObjects;

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ShowDataGridnformations();
        }

        private void ShowDataGridnformations()
        {
            GetClientsRequestsTable client = new GetClientsRequestsTable();

            List<ClientsRequestTable> data = client.GetClientsRequests(mainTextBox.Text, Convert.ToInt32(mainComboBox.SelectedValue));

            mainDataGrid.ItemsSource = data;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetStatuses status = new GetStatuses();

            mainComboBox.ItemsSource = status.GetStatus();
            mainComboBox.SelectedValuePath = "Id";
            mainComboBox.DisplayMemberPath = "Name";

            ShowDataGridnformations();
        }

        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDataGridnformations();
        }

        private void mainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowDataGridnformations();
        }

        private void openApplication_Click(object sender, RoutedEventArgs e)
        {
            var clientData = mainDataGrid.SelectedItem as ClientsRequestTable;

            clientsChangeDataControl control = new clientsChangeDataControl(clientData);
            addAndChangeForm form = new addAndChangeForm(control);
            control.CloseUC += ShowDataGridnformations;
            control.CloseUC += _HideObjects;
            control.CloseUC += RequesInfo;
            form.ShowDialog();
        }
    }
}
