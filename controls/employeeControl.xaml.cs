using finalExam_diplom_.classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            showInformationForDataGrid.information("SELECT " +
                                                    "e.id, e.employee_number, e.first_name || ' ' || e.last_name || ' ' || e.middle_name as fio, " +
                                                    "e.age, e.position, e.experience, e.phone_number, s.name as status " +
                                                    "FROM employees e LEFT JOIN employees_status s ON e.status_id = s.id", employeeDataGrid);
        }

        private void employeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            showInformationForDataGrid.information("SELECT " +
                                        "e.id, e.employee_number, e.first_name || ' ' || e.last_name || ' ' || e.middle_name as fio, " +
                                        "e.age, e.position, e.experience, e.phone_number, s.name as status " + "\n" +
                                        "FROM employees e " + "\n" +
                                        "LEFT JOIN employees_status s ON e.status_id = s.id" + "\n" +
                                        "WHERE e.employee_number || e.first_name || e.last_name || e.middle_name || e.position || s.name ILIKE '%" + employeeTextBox.Text + "%'", employeeDataGrid);
        }
    }
}
