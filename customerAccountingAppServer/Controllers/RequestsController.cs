using customerAccountingAppServer.Classes;
using customerAccountingAppServer.Classes.databaseTables;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestsController : ControllerBase
    {
        string sql = "SELECT cr.id, " +
                                        "CONCAT(c.first_name, ' ', c.last_name, ' ', c.middle_name) as fio," +
                                        "CONCAT('г.', a.city, ', ул.', a.street, ', ', a.house_number, ', кв.', ap.number_apartment) as address," +
                                        "r.name, ct.name, dr.name, cr.description, cr.status_id, s.name as status, cr.priority_id, p.name as priority, " +
                                        "cr.manager_id, d.employee_id as manager, " +
                                        "cr.master_id, CONCAT(e.first_name, ' ', e.last_name, ' ', e.middle_name) as master, cr.created_time, " +
                                        "cr.visit_time, cr.closed_time, cr.master_comment " +
                                        "FROM clients_requests cr " +
                                "LEFT JOIN clients c ON cr.client_id = c.id " +
                                "LEFT JOIN apartments ap ON cr.apartment_id = ap.id " +
                                "LEFT JOIN addresses a ON ap.address_id = a.id " +
                                "LEFT JOIN statuses s ON cr.status_id = s.id " +
                                "LEFT JOIN priorities p ON cr.priority_id = p.id " +
                                "LEFT JOIN employees e ON cr.master_id = e.id " +
                                "LEFT JOIN dispatchers d ON e.id = d.id " +
                                "LEFT JOIN categories ct ON cr.category_id = ct.id " +
                                "LEFT JOIN reasons r ON ct.reason_id = r.id " +
                                "LEFT JOIN duration dr ON cr.duration_id = dr.id WHERE cr.status_id = @status_id";

        [HttpGet()]
        public async Task<ActionResult<List<ClientsRequestTable>>> GetClientsRequests([FromQuery] int? statusId)
        {
            List<ClientsRequestTable> tableData = new List<ClientsRequestTable>();

            DataBaseConnection connection = new DataBaseConnection();

            await using(NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@status_id", statusId.Value);

                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new ClientsRequestTable
                            {
                                Id = reader.GetInt32(0),
                                Fio = reader[1].ToString(),
                                Address = reader[2].ToString(),
                                Reason = reader[3].ToString(),
                                Category = reader[4].ToString(),
                                Duration = reader[5].ToString(),
                                Description = reader[6].ToString(),
                                Status_id = reader.GetInt32(7),
                                Status = reader[8].ToString(),
                                Priority_id = reader.GetInt32(9),
                                Priority = reader[10].ToString(),
                                Manager_id = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                Manager = reader[12].ToString(),
                                Master_id = reader.IsDBNull(13) ? null : reader.GetInt32(13),
                                Master = reader[14].ToString(),
                                Created_time = reader.IsDBNull(15) ? null : reader.GetDateTime(15),
                                Visit_time = reader.IsDBNull(16) ? null : reader.GetDateTime(16),
                                Closed_time = reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                                Master_comment = reader[18].ToString()
                            });
                        }
                    }
                    return tableData;
                }

            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ClientsRequestTable>>> GetClientsRequestsSearch([FromQuery] int? statusId, [FromQuery] string? search)
        {
            List<ClientsRequestTable> tableData = new List<ClientsRequestTable>();

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                con.OpenAsync();

                string request = sql + " AND CONCAT(c.first_name, c.last_name, c.middle_name, a.street, " +
                "a.house_number, ap.number_apartment, cr.description, p.name, r.name, ct.name, dr.name) ILIKE @search";

                await using(NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@status_id", statusId.Value);
                    command.Parameters.AddWithValue("@search", $"%{search}%");

                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tableData.Add(new ClientsRequestTable
                            {
                                Id = reader.GetInt32(0),
                                Fio = reader[1].ToString(),
                                Address = reader[2].ToString(),
                                Reason = reader[3].ToString(),
                                Category = reader[4].ToString(),
                                Duration = reader[5].ToString(),
                                Description = reader[6].ToString(),
                                Status_id = reader.GetInt32(7),
                                Status = reader[8].ToString(),
                                Priority_id = reader.GetInt32(9),
                                Priority = reader[10].ToString(),
                                Manager_id = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                Manager = reader[12].ToString(),
                                Master_id = reader.IsDBNull(13) ? null : reader.GetInt32(13),
                                Master = reader[14].ToString(),
                                Created_time = reader.IsDBNull(15) ? null : reader.GetDateTime(15),
                                Visit_time = reader.IsDBNull(16) ? null : reader.GetDateTime(16),
                                Closed_time = reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                                Master_comment = reader[18].ToString()
                            });
                        }
                    }
                    return tableData;
                }

            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateClientRequestAsync([FromBody] ClientsRequestTable ClientData)
        {
            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                con.OpenAsync();

                string request = "UPDATE clients_requests SET status_id = 2, priority_id = @priority_id, manager_id = @manager_id, master_id = @master_id, visit_time = @visit_time WHERE id = @id";

                await using(NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@id", ClientData.Id);
                    command.Parameters.AddWithValue("@priority_id", ClientData.Priority_id);
                    command.Parameters.AddWithValue("@manager_id", ClientData.Manager_id);
                    command.Parameters.AddWithValue("@master_id", ClientData.Master_id);
                    command.Parameters.AddWithValue("@visit_time", ClientData.Visit_time);

                    await command.ExecuteNonQueryAsync();

                    return StatusCode(200);
                }
            }
        }
    }
}
