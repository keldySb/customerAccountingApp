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

        private async void ShowDataGridnformations()
        {
            if (string.IsNullOrWhiteSpace(mainTextBox.Text) == true)
            {
                GetClientsRequestsTable client = new GetClientsRequestsTable();

                List<ClientsRequestTable> dataStatus = await client.GetClientsRequestsAsync(Convert.ToInt32(mainComboBox.SelectedValue));

                mainDataGrid.ItemsSource = dataStatus;
            } else
            {
                GetClientsRequestsTable client = new GetClientsRequestsTable();

                List<ClientsRequestTable> dataSearch = await client.GetClientsRequestsSearchAsync(Convert.ToInt32(mainComboBox.SelectedValue), mainTextBox.Text);

                mainDataGrid.ItemsSource = dataSearch;
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DictionaryClass status = new DictionaryClass();

            mainComboBox.ItemsSource = await status.GetStatusAsync();
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
