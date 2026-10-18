using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace finalExam_diplom_.controls
{
    public partial class clientsChangeDataControl : UserControl
    {
        private ClientsRequestTable? _clientData;
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

        private void LoadClienteData()
        {
            GetStatuses status = new GetStatuses();

            priorityComboBox.ItemsSource = status.GetPriority();
            priorityComboBox.SelectedValuePath = "Id";
            priorityComboBox.DisplayMemberPath = "Name";

            statusComboBox.ItemsSource = status.GetStatus();
            statusComboBox.SelectedValuePath = "Id";
            statusComboBox.DisplayMemberPath = "Name";


            masterComboBox.ItemsSource = status.GetMaster();
            masterComboBox.SelectedValuePath = "Id";
            masterComboBox.DisplayMemberPath = "Name";

            managerComboBox.ItemsSource = status.GetManager();
            managerComboBox.SelectedValuePath = "Id";
            managerComboBox.DisplayMemberPath = "Name"; 

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

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (visitTimeDatePicker.SelectedDate != null && masterComboBox.SelectedValue != null)
            {
                if (visitTimeDatePicker.SelectedDate > DateTime.Now)
                {
                    ClientsRequestTable NewData = new ClientsRequestTable(_clientData.Id, null, null, null, null, null, Convert.ToInt32(priorityComboBox.SelectedValue), null, CurrentUser.Id, null, Convert.ToInt32(masterComboBox.SelectedValue), null, null, visitTimeDatePicker.SelectedDate, null, null);
                    EmployeesTable NewEmployeeData = new EmployeesTable(Convert.ToInt32(masterComboBox.SelectedValue), null, null, null, null, null, null, null, null, null);

                    UpdateInformation information = new UpdateInformation();

                    information.UpdateClientRequest(NewData);
                    information.UpdateEmployeeRequest(NewEmployeeData);

                    Raise();
                }
                else { MessageBox.Show("Дата визита не может быть меньше текущей даты"); }

            } else { MessageBox.Show("Не все поля заполнены"); }

        }
    }
}
