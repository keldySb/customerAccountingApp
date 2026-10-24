using BCrypt.Net;
using customerAccountingAppServer.Classes.databaseTables;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Classes
{
    public class AuthorizationClass()
    {
        public async Task<DispatchersTable> CheckUsers(string login, string? password)
        {
            DispatchersTable? tableData = null;

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = "SELECT * FROM dispatchers WHERE login = @login";

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
                                tableData.Employee_id = reader.GetInt32(1);
                            }
                        }
                    }
                    return tableData;
                }
            }

        }
    }
}
