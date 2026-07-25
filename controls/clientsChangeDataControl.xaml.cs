using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    public partial class clientsChangeDataControl : UserControl
    {
        private ClientsRequestTable _clientData;
        public clientsChangeDataControl(ClientsRequestTable clientData)
        {
            InitializeComponent();

            _clientData = clientData;
            LoadClienteData();
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


            clientTextBox.Text = _clientData.Fio;
            addressTextBox.Text = _clientData.Address;
            statusComboBox.SelectedValue = _clientData.Status_id;
            createdTimeDatePicker.SelectedDate = _clientData.Created_time;
            priorityComboBox.SelectedValue = _clientData.Priority_id;
            masterComboBox.SelectedValue = _clientData.Master_id;
            visitTimeDatePicker.SelectedDate = _clientData.Visit_time;
            closedTimeDatePicker.SelectedDate = _clientData.Closed_time;
            managerComboBox.SelectedValue = _clientData.Manager_id;
            descriptionTextBox.Text = _clientData.Description;
            masterCommentTextBox.Text = _clientData.Master_comment;
        }

        public event Action CloseUC = delegate { };

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
                    ClientsRequestTable NewData = new ClientsRequestTable(_clientData.Id, null, null, null, _clientData.Status_id, null, _clientData.Priority_id, null, _clientData.Manager_id, null, _clientData.Master_id, null, null, _clientData.Visit_time, null, null);

                    UpdateInformation information = new UpdateInformation();

                    information.UpdateClientRequest(NewData);

                    Raise();
                }
                else { MessageBox.Show("Дата визита не может быть меньше текущей даты"); }

            } else { MessageBox.Show("Не все поля заполнены"); }

        }
    }
}
