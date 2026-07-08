using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
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
    /// Логика взаимодействия для mainControl.xaml
    /// </summary>
    public partial class mainControl : UserControl
    {
        public mainControl()
        {
            InitializeComponent();
        }

        private void mainTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mainTextBox.Text == "Поиск")
                mainTextBox.Text = "";
        }

        private void mainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mainTextBox.Text))
                mainTextBox.Text = "Поиск"; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            showAllInformations();
        }

        private void showAllInformations()
        {
            showInformationForDataGrid.information("SELECT cr.id, " +
                                "c.first_name || ' ' || c.last_name || ' ' || c.middle_name as fio," +
                                "a.street || ', ' || a.house_number || ', ' || a.number_apartment as address," +
                                "e.age, cr.description, s.name as status, p.name as priority, " +
                                "e.first_name || ' ' || e.last_name || ' ' || e.middle_name as master, cr.created_time, " +
                                "cr.visit_time, cr.closed_time, cr.master_comment " +
                           "FROM clients_requests cr " +
                           "LEFT JOIN clients c ON cr.client_id = c.id " +
                           "LEFT JOIN addresses a ON cr.apartment_id = a.id " +
                           "LEFT JOIN statuses s ON cr.status_id = s.id " +
                           "LEFT JOIN priorities p ON cr.priority_id = p.id " +
                           "LEFT JOIN employees e ON cr.master_id = e.id", mainDataGrid);

            showInformationForComboBox.information("SELECT id, name FROM statuses", mainComboBox);
        }

        private void mainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            showInformationForDataGrid.information("SELECT " +
                                            "c.first_name || ' ' || c.last_name || ' ' || c.middle_name as fio," +
                                            "a.street || ', ' || a.house_number || ', ' || a.number_apartment as address," +
                                            "e.age, cr.description, s.name as status, p.name as priority, " +
                                            "e.first_name || ' ' || e.last_name || ' ' || e.middle_name as master, cr.created_time, " +
                                            "cr.visit_time, cr.closed_time, cr.master_comment" + "\n" +
                                        "FROM clients_requests cr " + "\n" +
                                        "LEFT JOIN clients c ON cr.client_id = c.id " + "\n" +
                                        "LEFT JOIN addresses a ON cr.apartment_id = a.id " + "\n" +
                                        "LEFT JOIN statuses s ON cr.status_id = s.id " + "\n" +
                                        "LEFT JOIN priorities p ON cr.priority_id = p.id " + "\n" +
                                        "LEFT JOIN employees e ON cr.master_id = e.id" + "\n" +
                                        "WHERE c.first_name || c.last_name || c.middle_name || a.street || a.house_number " +
                                        "|| a.number_apartment || cr.description || e.first_name || e.last_name || e.middle_name ILIKE '%" + mainTextBox.Text + "%'", mainDataGrid);
        }
    }
}
