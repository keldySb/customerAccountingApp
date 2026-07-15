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
                                                    "e.id, e.employee_number, e.first_name || ' ' || e.last_name || ' ' || e.middle_name as fio, " +
                                                    "e.age, e.position, e.experience, e.phone_number, s.name as status " +
                                                    "FROM employees e LEFT JOIN employees_status s ON e.status_id = s.id", employeeDataGrid);
        }

        private void employeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            showInformationEmployeeForDataGrid.information("SELECT " +
                                        "e.id, e.employee_number, e.first_name || ' ' || e.last_name || ' ' || e.middle_name as fio, " +
                                        "e.age, e.position, e.experience, e.phone_number, s.name as status " + "\n" +
                                        "FROM employees e " + "\n" +
                                        "LEFT JOIN employees_status s ON e.status_id = s.id" + "\n" +
                                        "WHERE e.employee_number || e.first_name || e.last_name || e.middle_name || e.position || s.name ILIKE '%" + employeeTextBox.Text + "%'", employeeDataGrid);
        }


        private void employeeDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var empData = employeeDataGrid.SelectedItem as employeesTable;

            employeeOpenDataControl control = new employeeOpenDataControl(empData);
            addAndChangeForm form = new addAndChangeForm(control);
            form.ShowDialog();
        }
    }
}
