using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using finalExam_diplom_.classes.databaseTables;
using Npgsql;

namespace finalExam_diplom_.classes
{
    public class showInformationEmployeeForDataGrid
    {
        public static void information(string query, DataGrid dataGrid) 
        {
            List<employeesTable> tableData = new List<employeesTable>();

            dataBaseConnection connection = new dataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
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
        public static void information(string query, DataGrid dataGrid, int ?statusId = null)
        {
            List<clientsRequestTable> tableData = new List<clientsRequestTable>();

            dataBaseConnection connection = new dataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@status_id", statusId);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(new clientsRequestTable()
                        {
                            id = reader.GetInt32(0),
                            fio = reader[1].ToString(),
                            address = reader[2].ToString(),
                            description = reader[3].ToString(),
                            status = reader[4].ToString(),
                            priority = reader[5].ToString(),
                            manager = reader[6].ToString(),
                            master = reader[7].ToString(),
                            created_time = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                            visit_time = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                            closed_time = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                            master_comment = reader[11].ToString()
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
            dataBaseConnection connection = new dataBaseConnection();

            List<dictionaryItems> tableData = new List<dictionaryItems>();

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
