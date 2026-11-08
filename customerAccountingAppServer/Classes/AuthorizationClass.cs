using customerAccountingAppServer.Classes.databaseTables;
using Npgsql;

namespace customerAccountingAppServer.Classes
{
    public class AuthorizationClass
    {
        private readonly DataBaseConnection _connection;

        public AuthorizationClass(DataBaseConnection connection)
        {
            _connection = connection;
        }

        public async Task<DispatchersTable?> CheckUsers(string login, string? password)
        {
            DispatchersTable? tableData = null;

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string sql = "SELECT id, CONCAT_WS(' ', last_name, first_name, middle_name) AS fio, " +
                    "login, password " +
                    "FROM dispatchers " +
                    "WHERE login = @login";

                await using(NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@login", login);

                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            if (BCrypt.Net.BCrypt.Verify(password, reader[3].ToString()) == true)
                            {
                                tableData = new DispatchersTable();
                                tableData.Id = reader.GetInt32(0);
                                tableData.FIO = reader[1].ToString();
                            }
                        }
                    }
                    return tableData;
                }
            }

        }
    }
}
