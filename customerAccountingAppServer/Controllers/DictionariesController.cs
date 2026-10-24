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
        [HttpGet("statuses")]
        public async Task<ActionResult<List<DictionaryItems>>> GetStatus()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT id, name FROM statuses", con))
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

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
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
        [HttpGet("masters")]
        public async Task<ActionResult<List<DictionaryItems>>> GetMaster()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT id, CONCAT(first_name, ' ', last_name, ' ', middle_name) as name FROM employees" +
                " WHERE NOT position_id = 2 AND status_id = 1", con))
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

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT d.id," +
                                    " CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as name" +
                                    " FROM dispatchers d" +
                                    " LEFT JOIN employees e ON d.employee_id = e.id", con))
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

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
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
        public async Task<ActionResult<List<DictionaryItems>>> GetCategory()
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand("SELECT id, name FROM categories", con))
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
    }
}
