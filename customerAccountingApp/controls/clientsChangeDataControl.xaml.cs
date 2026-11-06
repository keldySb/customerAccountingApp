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
            int? statusId = _clientData.Status_id;

            if (statusId == 1 || statusId == 3)
            {
                addTaskButton.Visibility = Visibility.Visible;
                dispatcherDescriptionTextBox.IsEnabled = true;
                priorityComboBox.IsEnabled = true;
                statusComboBox.IsEnabled = true;
                plannedTimeDatePicker.IsEnabled = true;
            }

            bool ShowEmployeeComboBox = statusId == 1 || (statusId == 3 && _clientData.Uk_decision == 2);
            employeeLabel.Visibility = ShowEmployeeComboBox ? Visibility.Visible : Visibility.Collapsed;
            employeeComboBox.Visibility = ShowEmployeeComboBox ? Visibility.Visible : Visibility.Collapsed;

            bool showDispatcher = statusId == 2 || statusId == 3 || statusId == 4 || statusId == 5;
            dispatcherLabel.Visibility = showDispatcher ? Visibility.Visible : Visibility.Collapsed;
            dispatcherComboBox.Visibility = showDispatcher ? Visibility.Visible : Visibility.Collapsed;
            dispatcherDescriptionTextBox.IsEnabled = showDispatcher ? IsEnabled == false : IsEnabled == true;

            bool showOrganization = statusId == 3;
            organizationDescriptionLabel.Visibility = showOrganization ? Visibility.Visible : Visibility.Collapsed;
            organizationDescriptionTextBox.Visibility = showOrganization ? Visibility.Visible : Visibility.Collapsed;
            decisionLabel.Visibility = showOrganization ? Visibility.Visible : Visibility.Collapsed;
            decisionTextBox.Visibility = showOrganization ? Visibility.Visible : Visibility.Collapsed;


            bool showClosedTime = statusId == 5;
            closedTimeLabel.Visibility = showClosedTime ? Visibility.Visible : Visibility.Collapsed;
            closedTimeDatePicker.Visibility = showClosedTime ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task LoadClienteData()
        {
            DictionaryClass dictionary = new DictionaryClass();

            priorityComboBox.ItemsSource = await dictionary.GetPriorityAsync();
            priorityComboBox.SelectedValuePath = "Id";
            priorityComboBox.DisplayMemberPath = "Name";

            statusComboBox.ItemsSource = await dictionary.GetStatusAsync(true);
            statusComboBox.SelectedValuePath = "Id";
            statusComboBox.DisplayMemberPath = "Name";

            employeeComboBox.ItemsSource = await dictionary.GetMasterAsync();
            employeeComboBox.SelectedValuePath = "Id";
            employeeComboBox.DisplayMemberPath = "Name";

            dispatcherComboBox.ItemsSource = await dictionary.GetManagerAsync();
            dispatcherComboBox.SelectedValuePath = "Id";
            dispatcherComboBox.DisplayMemberPath = "Name";

            dispatcherComboBox.SelectedValue = CurrentUser.Employee_id;

            organizationTextBox.Text = _clientData.Organization;
            organizationDescriptionTextBox.Text = _clientData.Organization_Comment;

            decisionTextBox.Text = _clientData.Uk_decision == 1
            ? "Проблема на стороне УК"
            : _clientData.Uk_decision == 2
                ? "Проблема на нашей стороне"
                : "Решение не указано";

            reasonTextBox.Text = _clientData.Reason;
            categoryTextBox.Text = _clientData.Category;
            durationTextBox.Text = _clientData.Duration;
            clientTextBox.Text = _clientData.Fio;
            addressTextBox.Text = _clientData.Address;
            clientDescriptionTextBox.Text = _clientData.Client_description;
            dispatcherDescriptionTextBox.Text = _clientData.Dispatcher_description;
            employeeComboBox.SelectedValue = _clientData.Employee_id;
            createdTimeDatePicker.SelectedDate = _clientData.Created_time;
            plannedTimeDatePicker.SelectedDate = _clientData.Planned_time;
            closedTimeDatePicker.SelectedDate = _clientData.Closed_time;

            statusComboBox.SelectedValue = _clientData.Status_id;
            priorityComboBox.SelectedValue = _clientData.Priority_id;
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
            if (plannedTimeDatePicker.SelectedDate != null && priorityComboBox.SelectedValue != null && statusComboBox.SelectedValue != null)
            {
                if (plannedTimeDatePicker.SelectedDate > DateTime.Now)
                {
                    ClientsRequestTable NewData = new ClientsRequestTable();

                    NewData.Id = _clientData.Id;
                    NewData.Status_id = (int)statusComboBox.SelectedValue;
                    NewData.Priority_id = (int)priorityComboBox.SelectedValue;
                    NewData.Dispatcher_id = CurrentUser.Employee_id;
                    NewData.Organization_id = _clientData.Organization_id;
                    NewData.Employee_id = employeeComboBox.SelectedValue as int?;
                    NewData.Planned_time = plannedTimeDatePicker.SelectedDate;
                    NewData.Dispatcher_description = dispatcherDescriptionTextBox.Text;
                    NewData.Uk_decision = _clientData.Uk_decision;

                    var statusRequest = await _httpClient.PostAsJsonAsync("api/requests", NewData);

                    if (NewData.Status_id == 3 && NewData.Uk_decision == 2 && NewData.Employee_id == null)
                    {
                        MessageBox.Show("Необходимо выбрать сотрудника");
                        return;
                    }

                    if (statusRequest.IsSuccessStatusCode)
                    {
                        Raise();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось сохранить изменения");
                    }
                }
                else { MessageBox.Show("Дата визита не может быть меньше текущей даты"); }

            } else { MessageBox.Show("Не все поля заполнены"); }

        }
    }
}
