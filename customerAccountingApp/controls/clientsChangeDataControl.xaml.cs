using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    public partial class clientsChangeDataControl : UserControl
    {
        private ClientsRequestTable? _clientData;

        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7070/")
        };
        public clientsChangeDataControl(ClientsRequestTable clientData)
        {
            InitializeComponent();

            _clientData = clientData;
            LoadClienteData();
            SetVisibility();
        }

        private void SetVisibility()
        {
            if (_clientData.Status_id == 1)
            {
                addTaskButton.Visibility = Visibility.Visible;
                masterComboBox.IsEnabled = true;
                priorityComboBox.IsEnabled = true;
                visitTimeDatePicker.IsEnabled = true;
            }
        }

        private async Task LoadClienteData()
        {
            DictionaryClass dictionary = new DictionaryClass();

            priorityComboBox.ItemsSource = await dictionary.GetPriorityAsync();
            priorityComboBox.SelectedValuePath = "Id";
            priorityComboBox.DisplayMemberPath = "Name";

            statusComboBox.ItemsSource = await dictionary.GetStatusAsync();
            statusComboBox.SelectedValuePath = "Id";
            statusComboBox.DisplayMemberPath = "Name";


            masterComboBox.ItemsSource = await dictionary.GetMasterAsync();
            masterComboBox.SelectedValuePath = "Id";
            masterComboBox.DisplayMemberPath = "Name";

            managerComboBox.ItemsSource = await dictionary.GetManagerAsync();
            managerComboBox.SelectedValuePath = "Id";
            managerComboBox.DisplayMemberPath = "Name";



            reasonTextBox.Text = _clientData.Reason;
            categoryTextBox.Text = _clientData.Category;
            durationTextBox.Text = _clientData.Duration;
            clientTextBox.Text = _clientData.Fio;
            addressTextBox.Text = _clientData.Address;
            descriptionTextBox.Text = _clientData.Description;
            masterCommentTextBox.Text = _clientData.Master_comment;

            createdTimeDatePicker.SelectedDate = _clientData.Created_time;
            visitTimeDatePicker.SelectedDate = _clientData.Visit_time;
            closedTimeDatePicker.SelectedDate = _clientData.Closed_time;

            statusComboBox.SelectedValue = _clientData.Status_id;
            priorityComboBox.SelectedValue = _clientData.Priority_id;
            masterComboBox.SelectedValue = _clientData.Master_id;
            managerComboBox.SelectedValue = CurrentUser.Id;
        }

        public Action CloseUC;

        public void Raise()
        {
            CloseUC();
        }

        private void CloseUCButton_Click(object sender, RoutedEventArgs e)
        {
            Raise();
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (visitTimeDatePicker.SelectedDate != null && masterComboBox.SelectedValue != null)
            {
                if (visitTimeDatePicker.SelectedDate > DateTime.Now)
                {
                    ClientsRequestTable NewData = new ClientsRequestTable();
                    NewData.Id = _clientData.Id;
                    NewData.Priority_id = (int)priorityComboBox.SelectedValue;
                    NewData.Manager_id = CurrentUser.Id;
                    NewData.Master_id = (int?)masterComboBox.SelectedValue;
                    NewData.Visit_time = visitTimeDatePicker.SelectedDate;

                    EmployeesTable NewEmployeeData = new EmployeesTable();
                    NewEmployeeData.Id = Convert.ToInt32(masterComboBox.SelectedValue);

                    var statusRequest = await _httpClient.PostAsJsonAsync("api/requests", NewData);
                    
                    var statusEmployee = await _httpClient.PostAsJsonAsync("api/employees", NewEmployeeData);

                    Raise();
                }
                else { MessageBox.Show("Дата визита не может быть меньше текущей даты"); }

            } else { MessageBox.Show("Не все поля заполнены"); }

        }
    }
}
