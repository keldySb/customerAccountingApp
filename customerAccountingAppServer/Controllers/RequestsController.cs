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
                             "CONCAT(c.last_name, ' ', c.first_name, ' ', c.middle_name) as fio, a.id, " +
                             "CONCAT('г.', a.city, ', ул.', a.street, ', ', a.house_number, ', кв.', ap.number_apartment) as address, " +
                             "r.name, " +
                             "ct.name, " +
                             "dr.name, " +
                             "cr.client_description, cr.dispatcher_description, cr.status_id, s.name as status, " +
                             "cr.priority_id, p.name as priority, " +
                             "cr.dispatcher_id, " +
                             "CONCAT(emp.first_name, ' ', emp.last_name, ' ', emp.middle_name) as employee, " +
                             "COALESCE(cr.organization_id, a.organization_id) AS organization_id, o.name AS organization, " +
                             "cr.employee_id, " +
                             "CONCAT(m.first_name, ' ', m.last_name, ' ', m.middle_name) as master, " +
                             "cr.created_time, cr.start_time, " +
                             "cr.planned_completion_time, cr.closed_time, " +
                             "cr.organization_comment, cr.employee_comment, cr.uk_decision " +
             "FROM clients_requests cr " +
             "LEFT JOIN clients c ON cr.client_id = c.id " +
             "LEFT JOIN apartments ap ON cr.apartment_id = ap.id " +
             "LEFT JOIN addresses a ON ap.address_id = a.id " +
             "LEFT JOIN statuses s ON cr.status_id = s.id " +
             "LEFT JOIN priorities p ON cr.priority_id = p.id " +
             "LEFT JOIN employees emp ON cr.dispatcher_id = emp.id " +
             "LEFT JOIN organizations o ON o.id = COALESCE(cr.organization_id, a.organization_id) " +
             "LEFT JOIN employees m ON cr.employee_id = m.id " +
             "LEFT JOIN categories ct ON cr.category_id = ct.id " +
             "LEFT JOIN reasons r ON ct.reason_id = r.id " +
             "LEFT JOIN duration dr ON cr.duration_id = dr.id " +
             "WHERE (@status_id = 0 OR cr.status_id = @status_id)";

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
                                Address_id = reader.GetInt32(2),
                                Address = reader[3].ToString(),
                                Reason = reader[4].ToString(),
                                Category = reader[5].ToString(),
                                Duration = reader[6].ToString(),
                                Client_description = reader[7].ToString(),
                                Dispatcher_description = reader[8].ToString(),
                                Status_id = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                Status = reader[10].ToString(),
                                Priority_id = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                Priority = reader.IsDBNull(12) ? "Не указан" : reader[12].ToString(),
                                Dispatcher_id = reader.IsDBNull(13) ? null : reader.GetInt32(13),
                                Dispatcher = reader[14].ToString(),
                                Organization_id = reader.IsDBNull(15) ? null : reader.GetInt32(15),
                                Organization = reader[16].ToString(),
                                Employee_id = reader.IsDBNull(17) ? null : reader.GetInt32(17),
                                Employee = reader[18].ToString(),
                                Created_time = reader.IsDBNull(19) ? null : reader.GetDateTime(19),
                                Start_time = reader.IsDBNull(20) ? null : reader.GetDateTime(20),
                                Planned_time = reader.IsDBNull(21) ? null : reader.GetDateTime(21),
                                Closed_time = reader.IsDBNull(22) ? null : reader.GetDateTime(22),
                                Organization_Comment = reader[23].ToString(),
                                Employee_comment = reader[24].ToString(),
                                Uk_decision = reader.IsDBNull(25) ? null : reader.GetInt32(25)
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
                "a.house_number, ap.number_apartment, cr.client_description, p.name, r.name, ct.name, dr.name) ILIKE @search";

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
                                Address_id = reader.GetInt32(2),
                                Address = reader[3].ToString(),
                                Reason = reader[4].ToString(),
                                Category = reader[5].ToString(),
                                Duration = reader[6].ToString(),
                                Client_description = reader[7].ToString(),
                                Dispatcher_description = reader[8].ToString(),
                                Status_id = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                Status = reader[10].ToString(),
                                Priority_id = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                Priority = reader.IsDBNull(12) ? "Не указан" : reader[12].ToString(),
                                Dispatcher_id = reader.IsDBNull(13) ? null : reader.GetInt32(13),
                                Dispatcher = reader[14].ToString(),
                                Organization_id = reader.IsDBNull(15) ? null : reader.GetInt32(15),
                                Organization = reader[16].ToString(),
                                Employee_id = reader.IsDBNull(17) ? null : reader.GetInt32(17),
                                Employee = reader[18].ToString(),
                                Created_time = reader.IsDBNull(19) ? null : reader.GetDateTime(19),
                                Start_time = reader.IsDBNull(20) ? null : reader.GetDateTime(20),
                                Planned_time = reader.IsDBNull(21) ? null : reader.GetDateTime(21),
                                Closed_time = reader.IsDBNull(22) ? null : reader.GetDateTime(22),
                                Organization_Comment = reader[23].ToString(),
                                Employee_comment = reader[24].ToString(),
                                Uk_decision = reader.IsDBNull(25) ? null : reader.GetInt32(25)
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
                await con.OpenAsync();

                string request = "UPDATE clients_requests SET " +
                                 "status_id = @status_id, " +
                                 "priority_id = @priority_id, " +
                                 "dispatcher_id = @dispatcher_id, " +
                                 "organization_id = @organization_id, " +
                                 "start_time = CURRENT_TIMESTAMP, " +
                                 "planned_completion_time = @planned_completion_time, " +
                                 "dispatcher_description = @dispatcher_description, " +
                                 "employee_id = @employee_id, " +
                                 "uk_decision = @uk_decision " +
                                 "WHERE id = @id";

                await using (NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@id", ClientData.Id);
                    command.Parameters.AddWithValue("@status_id", ClientData.Status_id.Value);
                    command.Parameters.AddWithValue("@priority_id", ClientData.Priority_id.Value);
                    command.Parameters.AddWithValue("@dispatcher_id", (object ?)ClientData.Dispatcher_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("@organization_id", ClientData.Organization_id);
                    command.Parameters.AddWithValue("@employee_id", (object?)ClientData.Employee_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("@planned_completion_time", (object?)ClientData.Planned_time ?? DBNull.Value);
                    command.Parameters.AddWithValue("@dispatcher_description", (object?)ClientData.Dispatcher_description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@uk_decision", (object?)ClientData.Uk_decision ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();

                    return StatusCode(200);
                }
            }
        }
    }
}
