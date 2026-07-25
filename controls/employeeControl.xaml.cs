using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    /// <summary>
    /// Логика взаимодействия для employeeControl.xaml
    /// </summary>

    public partial class employeeControl : UserControl
    {
        public employeeControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            showDataGridInformation();
        }

        private void showDataGridInformation()
        {
            GetEmployeeTable table = new GetEmployeeTable();

            List<EmployeesTable> employee = table.GetEmployee(employeeTextBox.Text);
            employeeDataGrid.ItemsSource = employee;
        }

        private void employeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            showDataGridInformation();
        }

        private void openApplication_Click(object sender, RoutedEventArgs e)
        {
            var empData = employeeDataGrid.SelectedItem as EmployeesTable;

            employeeOpenDataControl control = new employeeOpenDataControl(empData);
            addAndChangeForm form = new addAndChangeForm(control);
            form.ShowDialog();
        }

        public event Action setTask = delegate { };

        private void setTask_Click(object sender, RoutedEventArgs e)
        {
            setTask();
        }
    }
}
