using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Npgsql;

namespace finalExam_diplom_.classes
{
    public class showInformationForDataGrid
    {
        public static void information(string query, DataGrid dataGrid) 
        { 
            dataBaseConnection connection = new dataBaseConnection();


            using (NpgsqlConnection con = connection.getConnection())
            {
                connection.getConnection().Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection.getConnection()))

                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dataGrid.ItemsSource = table.DefaultView;
                }
            }
        }
    };

    public class showInformationForComboBox
    {
        public static void information(string query, ComboBox comboBox)
        {
            dataBaseConnection connection = new dataBaseConnection();


            using (NpgsqlConnection con = connection.getConnection())
            {
                connection.getConnection().Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection.getConnection()))

                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    comboBox.ItemsSource = table.DefaultView;
                }
            }
        }
    }
}
