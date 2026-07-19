using System.Windows;
using System.Windows.Controls;
using finalExam_diplom_.classes.databaseTables;

namespace finalExam_diplom_.controls
{
    /// <summary>
    /// Логика взаимодействия для employeeChangeDataControl.xaml
    /// </summary>
    public partial class employeeOpenDataControl : UserControl
    {
        private employeesTable _empData;
        public employeeOpenDataControl(employeesTable empData)
        {
            InitializeComponent();
            _empData = empData;
            loadEmployeeData();
        }

        private void loadEmployeeData()
        {
            employeeNumberTextBox.Text = _empData.employee_number;
            fioTextBox.Text = _empData.fio;
            ageTextBox.Text = _empData.age.ToString();
            positionTextBox.Text = _empData.position;
            experienceTextBox.Text = _empData.experience.ToString();
            phoneNumberTextBox.Text = _empData.phone_number;
            statusTextBox.Text = _empData.status;
        }

        public event Action closeUC = delegate { };

        private void closeUCButton_Click(object sender, RoutedEventArgs e)
        {
            closeUC();
        }
    }
}
