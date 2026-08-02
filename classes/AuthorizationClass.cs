using finalExam_diplom_.classes.databaseTables;
using Npgsql;
using System.Windows;

namespace finalExam_diplom_.classes
{
    public class AuthorizationClass()
    { 
        public DispatchersTable CheckUsers(string login, string? password) 
        {
            DispatchersTable? tableData = null;

            DataBaseConnection connection = new DataBaseConnection();

            using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = "SELECT * FROM dispatchers WHERE login = @login";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@login", login);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                         if (BCrypt.Net.BCrypt.Verify(password, reader[3].ToString()) == true)
                         {
                              tableData = new DispatchersTable();
                              tableData.Id = reader.GetInt32(0);
                              tableData.Employee_id = reader.GetInt32(1);
                         }
                    }

                    return tableData;
                }
            }
        
        } 
    }
}
