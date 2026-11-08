using customerAccountingAppServer.Classes;
using customerAccountingAppServer.Classes.databaseTables;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/dictionaries")]
    public class DictionariesController : ControllerBase
    {
        private readonly DataBaseConnection _connection;

        public DictionariesController(DataBaseConnection connection)
        {
            _connection = connection;
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<List<DictionaryItems>>> GetStatus([FromQuery] bool changeStatus = false)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string sql = "SELECT id, name FROM statuses";

                if (changeStatus)
                {
                    sql += " WHERE name IN ('Направлено в УК', 'Выполнено', 'В работе')";
                }

                await using(NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new DictionaryItems(
                                reader.GetInt32(0),
                                reader[1].ToString()));
                        }
                    }
                    return Ok(tableData);
                }
            }
        }


        [HttpGet("priorities")]
        public async Task<ActionResult<List<DictionaryItems>>> GetPriority()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            await using(NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT id, name FROM priorities", con))
                {
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new DictionaryItems(
                                reader.GetInt32(0),
                                reader[1].ToString()));
                        }
                    }
                    return Ok(tableData);
                }
            }
        }

        [HttpGet("managers")]
        public async Task<ActionResult<List<DictionaryItems>>> GetManager()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            await using(NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT d.id ," +
                                    " CONCAT_WS(' ', d.last_name, d.first_name, d.middle_name) AS name" +
                                    " FROM dispatchers d", con))
                {
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new DictionaryItems(
                                reader.GetInt32(0),
                                reader[1].ToString()));
                        }
                    }
                    return Ok(tableData);
                }
            }
        }

        [HttpGet("reasons")]
        public async Task<ActionResult<List<DictionaryItems>>> GetReason()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand("SELECT id, name FROM reasons", con))
                {
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new DictionaryItems(
                                reader.GetInt32(0),
                                reader[1].ToString()));
                        }
                    }
                    return Ok(tableData);
                }
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<DictionaryItems>>> GetCategory([FromQuery] int? reason)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand("SELECT ct.id, ct.name FROM categories ct" +
                                                                                " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                                                                " WHERE r.id = @reason", con))
                {
                    if (reason != null)
                        command.Parameters.AddWithValue("@reason", (object?)reason ?? DBNull.Value);
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new DictionaryItems(
                                reader.GetInt32(0),
                                reader[1].ToString()));
                        }
                    }
                    return Ok(tableData);
                }
            }
        }
    }
}
