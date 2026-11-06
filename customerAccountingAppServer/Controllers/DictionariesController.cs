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
        public async Task<ActionResult<List<DictionaryItems>>> GetStatus([FromQuery] bool changeStatus = false)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
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
                " WHERE NOT position_id = 2", con))
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

                await using(NpgsqlCommand command = new NpgsqlCommand("SELECT d.employee_id ," +
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
        public async Task<ActionResult<List<DictionaryItems>>> GetCategory([FromQuery] int? reason)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand("SELECT ct.id, ct.name FROM categories ct" +
                                                                                " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                                                                " WHERE r.id = @reason", con))
                {
                    if (reason != null)
                        command.Parameters.AddWithValue("@reason", reason);
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


        [HttpGet("organization")]
        public async Task<ActionResult<List<DictionaryItems>>> GetOrganization([FromQuery] int? address_id)
        {
            List<DictionaryItems> tableData = new List<DictionaryItems>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand("SELECT o.id, o.name FROM organizations o" +
                                                                                " LEFT JOIN addresses a ON o.id = a.organization_id" +
                                                                                " WHERE a.id = @address_id", con))
                {
                    if (address_id != null)
                        command.Parameters.AddWithValue("@address_id", address_id);
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
