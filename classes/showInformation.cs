using finalExam_diplom_.classes.databaseTables;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.classes
{
    public class showInformationEmployeeForDataGrid
    {
        public static void information(string query, DataGrid dataGrid, string? search) 
        {
            List<employeesTable> tableData = new List<employeesTable>();

            dataBaseConnection connection = new dataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(search))
                        command.Parameters.AddWithValue("@search", $"%{search}%"); 

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new employeesTable()
                        {
                            id = reader.GetInt32(0),
                            employee_number = reader[1].ToString(),
                            fio = reader[2].ToString(),
                            age = reader.GetInt32(3),
                            position = reader[4].ToString(),
                            experience = reader.GetInt32(5),
                            phone_number = reader[6].ToString(),
                            status = reader[7].ToString()
                        });
                    }
                    dataGrid.ItemsSource = tableData;
                }
            }
        }
    };

    public class showInformationClientsREquestForDataGrid
    {
        public static void information(string query, DataGrid dataGrid, string? search)
        { 
            List<clientsRequestTable> tableData = new List<clientsRequestTable>();

            dataBaseConnection connection = new dataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(search))
                        command.Parameters.AddWithValue("@search", $"%{search}%");

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new clientsRequestTable()
                        {
                            id = reader.GetInt32(0),
                            fio = reader[1].ToString(),
                            address = reader[2].ToString(),
                            description = reader[3].ToString(),
                            status_id = reader.GetInt32(4),
                            status = reader[5].ToString(),
                            priority_id = reader.GetInt32(6),
                            priority = reader[7].ToString(),
                            manager_id = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                            manager = reader[9].ToString(),
                            master_id = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                            master = reader[11].ToString(),
                            created_time = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                            visit_time = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                            closed_time = reader.IsDBNull(14) ? null : reader.GetDateTime(14),
                            master_comment = reader[15].ToString()
                        });
                    }

                    dataGrid.ItemsSource = tableData;
                }
            }
        }
    };

    public class showInformationForComboBox
    {
        public static void information(string query, ComboBox comboBox)
        {
            List<dictionaryItems> tableData = new List<dictionaryItems>();

            dataBaseConnection connection = new dataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new dictionaryItems()
                        {
                            id = reader.GetInt32(0),
                            name = reader[1].ToString(),
                        });
                    }

                    comboBox.ItemsSource = tableData;
                    comboBox.DisplayMemberPath = "name";
                    comboBox.SelectedValuePath = "id";
                }
            }
        }
    }
}
