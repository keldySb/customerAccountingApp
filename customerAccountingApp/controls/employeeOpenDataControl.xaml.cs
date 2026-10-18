using System.Windows;
using System.Windows.Controls;
using finalExam_diplom_.classes.databaseTables;

namespace finalExam_diplom_.controls
{
    public partial class employeeOpenDataControl : UserControl
    {
        private EmployeesTable _empData;
        public employeeOpenDataControl(EmployeesTable empData)
        {
            InitializeComponent();
            _empData = empData;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            employeeNumberTextBox.Text = _empData.Employee_number;
            fioTextBox.Text = _empData.Fio;
            ageTextBox.Text = _empData.Age.ToString();
            positionTextBox.Text = _empData.Position;
            experienceTextBox.Text = _empData.Experience.ToString();
            phoneNumberTextBox.Text = _empData.Phone_number;
            statusTextBox.Text = _empData.Status;
        }

        public event Action CloseUC = delegate { };

        private void CloseUCButton_Click(object sender, RoutedEventArgs e)
        {
            CloseUC();
        }
    }
}
