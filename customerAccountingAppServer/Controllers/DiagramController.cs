using customerAccountingAppServer.Classes;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/count")]
    public class DiagramController : ControllerBase
    {
        private readonly string sql = "SELECT count(*), s.name  FROM clients_requests c" +
                                            " LEFT JOIN statuses s ON c.status_id = s.id" +
                                            " GROUP BY s.name";

        [HttpGet]
        public async Task<ActionResult<List<DiagramClass>>> GetCountRequests()
        {
            List<DiagramClass> diagram = new List<DiagramClass>();
            
            DataBaseConnection connection = new DataBaseConnection();

            await using (NpgsqlConnection con = connection.getConnection())
            {
                await con.OpenAsync();

                await using(NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
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
