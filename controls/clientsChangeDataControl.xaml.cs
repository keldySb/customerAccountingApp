using System.Windows;
using System.Windows.Controls;
using finalExam_diplom_.classes.databaseTables;

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
            clientTextBox.Text = _clientData.fio;
            addressTextBox.Text = _clientData.address;
            statusComboBox.Text = _clientData.status;
            createdTimeDatePicker.SelectedDate = _clientData.created_time;
            priorityComboBox.Text = _clientData.priority;
            masterComboBox.Text = _clientData.master;
            visitTimeDatePicker.SelectedDate = _clientData.visit_time;
            closedTimeDatePicker.SelectedDate = _clientData.closed_time;
            managerComboBox.Text = _clientData.manager;
            descriptionTextBox.Text = _clientData.description;
            masterCommentTextBox.Text = _clientData.master_comment;
        }
        private void closeFormButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
