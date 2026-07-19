using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    /// <summary>
    /// Логика взаимодействия для clientsChangeDataControl.xaml
    /// </summary>
    public partial class clientsChangeDataControl : UserControl
    {
        private clientsRequestTable _clientData;
        public clientsChangeDataControl(clientsRequestTable clientData)
        {
            InitializeComponent();

            _clientData = clientData;
            loadClienteData();
        }

        private void loadClienteData()
        {
            showInformationForComboBox.information("SELECT id, name FROM priorities", priorityComboBox);
            showInformationForComboBox.information("SELECT id, name FROM statuses", statusComboBox);
            showInformationForComboBox.information("SELECT id, first_name || ' ' || last_name || ' ' || middle_name as name FROM employees", masterComboBox);

            clientTextBox.Text = _clientData.fio;
            addressTextBox.Text = _clientData.address;
            statusComboBox.SelectedValue = _clientData.status_id;
            createdTimeDatePicker.SelectedDate = _clientData.created_time;
            priorityComboBox.SelectedValue = _clientData.priority_id;
            masterComboBox.SelectedValue = _clientData.master_id;
            visitTimeDatePicker.SelectedDate = _clientData.visit_time;
            closedTimeDatePicker.SelectedDate = _clientData.closed_time;
            managerComboBox.SelectedItem = _clientData.manager_id;
            descriptionTextBox.Text = _clientData.description;
            masterCommentTextBox.Text = _clientData.master_comment;
        }

        public event Action closeUC = delegate { };

        public void raise()
        {
            closeUC();
        }

        private void closeUCButton_Click(object sender, RoutedEventArgs e)
        {
            raise();
        }
    }
}
