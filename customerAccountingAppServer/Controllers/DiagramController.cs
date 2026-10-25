using customerAccountingAppServer.Classes;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/count")]
    public class DiagramController : ControllerBase
    {
        [HttpGet("statuses")]
        public async Task<ActionResult<List<DiagramClass>>> GetCountRequests([FromQuery] string? reason, [FromQuery] string? category)
        {
            List<DiagramClass> diagram = new List<DiagramClass>();
            
            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                string request = "";

                if (string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), s.name FROM clients_requests c" +
                                      " LEFT JOIN statuses s ON c.status_id = s.id"
                                    + " GROUP BY s.name";
                }
                else if (reason != null && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), s.name  FROM clients_requests c" +
                                        " LEFT JOIN statuses s ON c.status_id = s.id" +
                                        " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                        " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                        " WHERE r.name = @reason" +
                                        " GROUP BY s.name";
                }
                else if (reason != null && category != null)
                {
                    request = "SELECT count(*), s.name  FROM clients_requests c" +
                                    " LEFT JOIN statuses s ON c.status_id = s.id" +
                                    " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                    " WHERE ct.name = @category" +
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

            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                string request = "";

                if (string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(category))
                {                                         
                    request = "SELECT count(*), p.name  FROM clients_requests c" +
                                    " LEFT JOIN priorities p ON c.priority_id = p.id"
                                        + " GROUP BY p.name";
                }
                else if (reason != null && string.IsNullOrEmpty(category))
                {
                    request = "SELECT count(*), p.name  FROM clients_requests c" +
                                        " LEFT JOIN priorities p ON c.priority_id = p.id" +
                                        " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                        " LEFT JOIN reasons r ON ct.reason_id = r.id" +
                                        " WHERE r.name = @reason" +
                                        " GROUP BY p.name";
                } else if (reason != null && category != null)
                {
                    request = "SELECT count(*), p.name  FROM clients_requests c" +
                                    " LEFT JOIN priorities p ON c.priority_id = p.id" +
                                    " LEFT JOIN categories ct ON c.category_id = ct.id" +
                                    " WHERE ct.name = @category" +
                                    " GROUP BY p.name";
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
