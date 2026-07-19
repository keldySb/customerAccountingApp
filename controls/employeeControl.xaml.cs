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
            showInformationEmployeeForDataGrid.information("SELECT " +
                                                    "e.id, e.employee_number, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
                                                    "e.age, e.position, e.experience, e.phone_number, s.name as status " +
                                                    "FROM employees e LEFT JOIN employees_status s ON e.status_id = s.id", employeeDataGrid, null);
        }

        private void employeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sql = "SELECT " +
                             "e.id, e.employee_number, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
                             "e.age, e.position, e.experience, e.phone_number, s.name as status " +
                             "FROM employees e " +
                             "LEFT JOIN employees_status s ON e.status_id = s.id ";

            if (!string.IsNullOrWhiteSpace(employeeTextBox.Text))
            {
                sql += "WHERE CONCAT(e.employee_number, e.first_name, e.last_name, e.middle_name, e.position, s.name) ILIKE @search";
            }

            showInformationEmployeeForDataGrid.information(sql, employeeDataGrid, employeeTextBox.Text);
        }

        private void openApplication_Click(object sender, RoutedEventArgs e)
        {
            var empData = employeeDataGrid.SelectedItem as employeesTable;

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
