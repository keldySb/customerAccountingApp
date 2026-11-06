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
            "e.id, " +
            "CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as fio, " +
            "e.position_id, " +
            "p.name as position, " +
            "e.phone_number, " +
            "e.employee_status_id, " +
            "es.name as employee_status " +
            "FROM employees e " +
            "LEFT JOIN positions p ON e.position_id = p.id " +
            "LEFT JOIN employee_statuses es ON e.employee_status_id = es.id";

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
                            Position_Id = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            Position = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Phone_number = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Employee_status_id = reader.GetInt32(5),
                            Employee_status = reader.IsDBNull(6) ? null : reader.GetString(6)
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
                            Position_Id = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            Position = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Phone_number = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Employee_status_id = reader.GetInt32(5),
                            Employee_status = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }
                }
                return tableData;
            }
        }
    }
}

