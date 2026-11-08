using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace finalExam_diplom_.controls
{
    public partial class MainControl : UserControl
    {
        public Action? _HideObjects;
        public Action? RequesInfo;
        private DispatcherTimer timer;
        public bool ukWorks;

        public MainControl(Action? hideObjects)
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
            GetClientsRequestsTable client = new GetClientsRequestsTable();

            if (string.IsNullOrWhiteSpace(mainTextBox.Text))
            {
                List<ClientsRequestTable> dataStatus = await client.GetClientsRequestsAsync(Convert.ToInt32(mainComboBox.SelectedValue));
                mainDataGrid.ItemsSource = dataStatus;
            }
            else
            {
                List<ClientsRequestTable> dataStatus = await client.GetClientsRequestsSearchAsync(Convert.ToInt32(mainComboBox.SelectedValue), mainTextBox.Text);
                mainDataGrid.ItemsSource = dataStatus;
            }
        }

        private void UpdateVisible()
        {
            int? status_id = mainComboBox.SelectedValue as int?;

            Status.Visibility = status_id == 0 ? Visibility.Visible : Visibility.Collapsed;

            Organization_Comment.Visibility = status_id == 3 ? Visibility.Visible : Visibility.Collapsed;
            Uk_decision_text.Visibility = status_id == 3 ? Visibility.Visible : Visibility.Collapsed;

            Closed_time.Visibility = status_id == 5 ? Visibility.Visible : Visibility.Collapsed;
            Work_comment.Visibility = status_id == 5 || status_id == 4 || status_id == 3 ? Visibility.Visible : Visibility.Collapsed;
        }


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DictionaryClass status = new DictionaryClass();

            List<DictionaryItems> statuses = await status.GetStatusAsync(false);
            statuses.Insert(0, new DictionaryItems(0, "Все заявки"));
            mainComboBox.ItemsSource = statuses;
            mainComboBox.SelectedValuePath = "Id";
            mainComboBox.DisplayMemberPath = "Name";
            mainComboBox.SelectedIndex = 0;

            ShowDataGridnformations();
            UpdateVisible();
        }

        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDataGridnformations();
            UpdateVisible();
        }

        private void mainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowDataGridnformations();
        }

        private void openApplication_Click(object sender, RoutedEventArgs e)
        {
            var clientData = mainDataGrid.SelectedItem as ClientsRequestTable;

            if (clientData == null)
                return;

            clientsChangeDataControl control = new clientsChangeDataControl(clientData);
            addAndChangeForm form = new addAndChangeForm(control);
            control.CloseUC += ShowDataGridnformations;
            if (_HideObjects != null)
                control.CloseUC += _HideObjects;
            control.CloseUC += RequesInfo;
            form.ShowDialog();
        }
    }
}
