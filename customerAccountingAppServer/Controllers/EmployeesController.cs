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
                                  "e.id, e.employee_number, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
                                  "e.age, e.position_id, p.name as position, e.experience, e.phone_number, e.status_id, s.name as status " +
                                "FROM employees e" +
                                " LEFT JOIN employees_status s ON e.status_id = s.id" +
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
                            Employee_number = reader[1].ToString(),
                            Fio = reader[2].ToString(),
                            Age = reader.GetInt32(3),
                            Position_Id = reader.GetInt32(4),
                            Position = reader[5].ToString(),
                            Experience = reader.GetInt32(6),
                            Phone_number = reader[7].ToString(),
                            Status_Id = reader.GetInt32(8),
                            Status = reader[9].ToString()
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

                string request = sql + " WHERE CONCAT(e.employee_number, e.first_name, e.last_name, e.middle_name, p.name, s.name) ILIKE @search";

                await using (NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (await reader.ReadAsync())
                    {
                        tableData.Add(new EmployeesTable
                        {
                            Id = reader.GetInt32(0),
                            Employee_number = reader[1].ToString(),
                            Fio = reader[2].ToString(),
                            Age = reader.GetInt32(3),
                            Position_Id = reader.GetInt32(4),
                            Position = reader[5].ToString(),
                            Experience = reader.GetInt32(6),
                            Phone_number = reader[7].ToString(),
                            Status_Id = reader.GetInt32(8),
                            Status = reader[9].ToString()
                        });
                    }
                }
                return tableData;
            }
        }

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

