using customerAccountingAppServer.Classes;
using customerAccountingAppServer.Classes.databaseTables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly string sql = "SELECT " +
                                  "CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
                                  "e.age, e.position_id, p.name as position, e.phone_number" +
                                "FROM employees e" +
                                " LEFT JOIN positions p ON e.position_id = p.id";
        [HttpGet]
        public async Task<ActionResult<List<EmployeesTable>>> GetEmployeesAsync()
        {
            List<EmployeesTable> tableData = new List<EmployeesTable>();

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (await reader.ReadAsync())
                    {
                        tableData.Add(new EmployeesTable
                        {
                            Id = reader.GetInt32(0),
                            Fio = reader[1].ToString(),
                            Age = reader.GetInt32(2),
                            Position_Id = reader.GetInt32(3),
                            Position = reader[4].ToString(),
                            Phone_number = reader[5].ToString(),
                        });
                    }
                }
                return tableData;
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EmployeesTable>>> GetEmployeesSearchAsync([FromQuery] string? search)
        {
            List<EmployeesTable> tableData = new List<EmployeesTable>();

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string request = sql + " WHERE CONCAT(e.first_name, e.last_name, e.middle_name, p.name) ILIKE @search";

                await using (NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (await reader.ReadAsync())
                    {
                        tableData.Add(new EmployeesTable
                        {
                            Id = reader.GetInt32(0),
                            Fio = reader[1].ToString(),
                            Age = reader.GetInt32(2),
                            Position_Id = reader.GetInt32(3),
                            Position = reader[4].ToString(),
                            Phone_number = reader[5].ToString(),
                        });
                    }
                }
                return tableData;
            }
        }
        //что с этим денлать
        [HttpPost]
        public async Task<ActionResult> UpdateEmployeeRequestAsync([FromBody] EmployeesTable EmpData)
        {
            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                con.Open();

                string sql = $"UPDATE employees SET status_id = 2 WHERE id = @id";

                await using(NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@id", EmpData.Id);

                    await command.ExecuteNonQueryAsync();
                }

                return StatusCode(200);
            }
        }
    }
}

