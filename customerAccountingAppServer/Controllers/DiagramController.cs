using customerAccountingAppServer.Classes;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/count")]
    public class DiagramController : ControllerBase
    {
        private readonly DataBaseConnection _connection;

        public DiagramController(DataBaseConnection connection)
        {
            _connection = connection;
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<List<DiagramClass>>> GetCountRequests([FromQuery] int? time, [FromQuery] string? reason, [FromQuery] string? category)
        {
            List<DiagramClass> diagram = new List<DiagramClass>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                string request = "";
                string timeRequest = "";
                switch (time)
                {
                    case 1:
                        timeRequest = " AND c.created_time >= NOW() - INTERVAL '1 month'";
                        break;
                    case 2:
                        timeRequest = " AND c.created_time >= NOW() - INTERVAL '1 week'";
                        break;
                    case 3:
                        timeRequest = " AND c.created_time >= NOW() - INTERVAL '1 day'";
                        break;
                    default:
                        timeRequest = "";
                        break;
                }

                if (string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), s.name FROM clients_requests c" +
                                      " LEFT JOIN statuses s ON c.status_id = s.id" +
                                      " WHERE c.status_id = s.id" +
                                      timeRequest +
                                      " GROUP BY s.name";
                }
                else if (reason != null && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), s.name  FROM clients_requests c" +
                                        " LEFT JOIN statuses s ON c.status_id = s.id" +
                                        " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                        " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                        " WHERE r.name = @reason" +
                                        timeRequest +
                                        " GROUP BY s.name";
                }
                else if (reason != null && category != null)
                {
                    request = "SELECT count(*), s.name  FROM clients_requests c" +
                                    " LEFT JOIN statuses s ON c.status_id = s.id" +
                                    " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                    " WHERE ct.name = @category" +
                                    timeRequest +
                                    " GROUP BY s.name";
                }

                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    if (reason != null && string.IsNullOrEmpty(category))
                        command.Parameters.AddWithValue("@reason", reason);
                    if (reason != null && category != null)
                        command.Parameters.AddWithValue("@category", category);
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            diagram.Add(new DiagramClass
                            {
                                Count = reader.GetInt32(0),
                                Name = reader[1].ToString()
                            });
                        }
                    }
                    return Ok(diagram);
                }
            }
        }
        
        [HttpGet("priorities")]
        public async Task<ActionResult<List<DiagramClass>>> GetCountPriorities([FromQuery] string? reason, [FromQuery] string? category)
        {
            List<DiagramClass> diagram = new List<DiagramClass>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                string request = "";

                if (string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(category))
                {                                         
                    request = "SELECT count(*), COALESCE(p.name, 'Не указан') AS name  FROM clients_requests c" +
                                    " LEFT JOIN priorities p ON c.priority_id = p.id"
                                        + " GROUP BY COALESCE(p.name, 'Не указан')";
                }
                else if (reason != null && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), COALESCE(p.name, 'Не указан') AS name  FROM clients_requests c" +
                                        " LEFT JOIN priorities p ON c.priority_id = p.id" +
                                        " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                        " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                        " WHERE r.name = @reason" +
                                        " GROUP BY COALESCE(p.name, 'Не указан')";
                } else if (reason != null && category != null)
                {
                    request = "SELECT count(*), COALESCE(p.name, 'Не указан') AS name  FROM clients_requests c" +
                                    " LEFT JOIN priorities p ON c.priority_id = p.id" +
                                    " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                    " WHERE ct.name = @category" +
                                    " GROUP BY COALESCE(p.name, 'Не указан')";
                }

                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    if (reason != null && string.IsNullOrEmpty(category))
                        command.Parameters.AddWithValue("@reason", reason);
                    if (reason != null && category != null)
                        command.Parameters.AddWithValue("@category", category);
                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            diagram.Add(new DiagramClass
                            {
                                Count = reader.GetInt32(0),
                                Name = reader[1].ToString()
                            });
                        }
                    }
                    return Ok(diagram);
                }
            }
        }
    }

}
