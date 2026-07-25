using finalExam_diplom_.classes.databaseTables;
using Npgsql;

namespace finalExam_diplom_.classes
{
    public class AuthorizationClass()
    { 
        public List<DispatchersTable> CheckUsers(string login, string password) 
        {
            List<DispatchersTable> tableData = new List<DispatchersTable>();

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
                              tableData.Add(new DispatchersTable(reader.GetInt32(0), reader.GetInt32(1), reader[2].ToString(), reader[3].ToString()));
                         }
                    }

                    return tableData;
                }
            }
        
        } 
    }
}
