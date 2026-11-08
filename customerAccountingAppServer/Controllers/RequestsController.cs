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
        private readonly DataBaseConnection _connection;
        private readonly UkSiteClient _ukSiteClient;


        public RequestsController(
            DataBaseConnection connection,
            UkSiteClient ukSiteClient)
        {
            _connection = connection;
            _ukSiteClient = ukSiteClient;
        }

        string sql = "SELECT cr.id, " +
                             "CONCAT(c.last_name, ' ', c.first_name, ' ', c.middle_name) as fio, a.id, " +
                             "CONCAT('г.', a.city, ', ул.', a.street, ', ', a.house_number, ', кв.', ap.number_apartment) as address, " +
                             "r.name, " +
                             "ct.name, " +
                             "reg.name, " +
                             "cr.client_description, cr.dispatcher_description, cr.status_id, s.name as status, " +
                             "cr.priority_id, p.name as priority, " +
                             "cr.dispatcher_id, " +
                             "CONCAT(d.last_name, ' ', d.first_name, ' ', d.middle_name) as dispatcher, " +
                             "COALESCE(cr.organization_id, a.organization_id) AS organization_id, o.name AS organization, " +
                             "cr.created_time, cr.start_time, " +
                             "cr.planned_completion_time, cr.closed_time, " +
                             "cr.organization_comment, cr.uk_decision, cr.work_comment " +
             "FROM clients_requests cr " +
             "LEFT JOIN clients c ON cr.client_id = c.id " +
             "LEFT JOIN apartments ap ON cr.apartment_id = ap.id " +
             "LEFT JOIN addresses a ON ap.address_id = a.id " +
             "LEFT JOIN statuses s ON cr.status_id = s.id " +
             "LEFT JOIN priorities p ON cr.priority_id = p.id " +
             "LEFT JOIN dispatchers d ON cr.dispatcher_id = d.id " +
             "LEFT JOIN organizations o ON o.id = COALESCE(cr.organization_id, a.organization_id) " +
             "LEFT JOIN categories ct ON cr.category_id = ct.id " +
             "LEFT JOIN reasons r ON ct.reason_id = r.id " +
             "LEFT JOIN regularity reg ON cr.regularity_id = reg.id " +
             "WHERE (@status_id = 0 OR cr.status_id = @status_id)";

        [HttpGet()]
        public async Task<ActionResult<List<ClientsRequestTable>>> GetClientsRequests([FromQuery] int? statusId)
        {
            List<ClientsRequestTable> tableData = new List<ClientsRequestTable>();

            await using(NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@status_id", statusId ?? 0);

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
                                Regularity = reader[6].ToString(),
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
                                Created_time = reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                                Start_time = reader.IsDBNull(18) ? null : reader.GetDateTime(18),
                                Planned_time = reader.IsDBNull(19) ? null : reader.GetDateTime(19),
                                Closed_time = reader.IsDBNull(20) ? null : reader.GetDateTime(20),
                                Organization_Comment = reader[21].ToString(),
                                Uk_decision = reader.IsDBNull(22) ? null : reader.GetInt32(22),
                                Work_comment = reader.IsDBNull(23) ? null : reader[23].ToString()
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

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string request = sql + " AND CONCAT(c.first_name, c.last_name, c.middle_name, a.street, " +
                "a.house_number, ap.number_apartment, cr.client_description, p.name, r.name, ct.name, reg.name) ILIKE @search";

                await using(NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@status_id", statusId ?? 0);
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
                                Regularity = reader[6].ToString(),
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
                                Created_time = reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                                Start_time = reader.IsDBNull(18) ? null : reader.GetDateTime(18),
                                Planned_time = reader.IsDBNull(19) ? null : reader.GetDateTime(19),
                                Closed_time = reader.IsDBNull(20) ? null : reader.GetDateTime(20),
                                Organization_Comment = reader[21].ToString(),
                                Uk_decision = reader.IsDBNull(22) ? null : reader.GetInt32(22),
                                Work_comment = reader.IsDBNull(23) ? null : reader[23].ToString()
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

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string request = "UPDATE clients_requests SET " +
                                 "status_id = @status_id, " +
                                 "priority_id = @priority_id, " +
                                 "dispatcher_id = @dispatcher_id, " +
                                 "organization_id = @organization_id, " +
                                 "start_time = CASE " +
                                    "WHEN @status_id IN(4, 5) " +
                                    "THEN COALESCE(start_time, CURRENT_TIMESTAMP) " +
                                    "ELSE start_time " +
                                    "END, " +

                                "closed_time = CASE " +
                                   "WHEN @status_id = 5 " +
                                   "THEN COALESCE(closed_time, CURRENT_TIMESTAMP) " +
                                   "ELSE closed_time " +
                                   "END, " +
                                 "planned_completion_time = @planned_completion_time, " +
                                 "dispatcher_description = @dispatcher_description, " +
                                 "uk_decision = @uk_decision " +
                                 "WHERE id = @id";

                await using (NpgsqlCommand command = new NpgsqlCommand(request, con))
                {
                    command.Parameters.AddWithValue("@id", ClientData.Id);
                    command.Parameters.AddWithValue("@status_id", ClientData.Status_id ?? 0);
                    command.Parameters.AddWithValue("@priority_id", (object?)ClientData.Priority_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("@dispatcher_id", (object ?)ClientData.Dispatcher_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("@organization_id", (object?)ClientData.Organization_id ?? DBNull.Value);
                    command.Parameters.AddWithValue("@planned_completion_time", (object?)ClientData.Planned_time ?? DBNull.Value);
                    command.Parameters.AddWithValue("@dispatcher_description", (object?)ClientData.Dispatcher_description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@uk_decision", (object?)ClientData.Uk_decision ?? DBNull.Value);

                    int isRequestExists = await command.ExecuteNonQueryAsync();

                    if (isRequestExists == 0)
                        return NotFound();

                    if (ClientData.Status_id == 2)
                    {
                        await _ukSiteClient.SendRequestAsync(ClientData.Id);
                    }

                    return Ok();
                }
            }
        }

        [HttpPost("uk-response")]
        public async Task<ActionResult> ReceiveUkResponse([FromBody] UkResponseDto data)
        {
            if (data == null ||
                data.RequestId <= 0 ||
                data.UkDecision is not (1 or 2) ||
                string.IsNullOrWhiteSpace(data.Comment))
            {
                return BadRequest(new
                {
                    message = "Некорректный ответ УК."
                });
            }

            string comment = data.Comment.Trim();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string sql =
                    "UPDATE clients_requests " +
                    "SET uk_decision = @uk_decision, " +
                    "organization_comment = @comment, " +
                    "status_id = 3 " +
                    "WHERE id = @id";

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@id", data.RequestId);
                    command.Parameters.AddWithValue("@uk_decision", data.UkDecision);
                    command.Parameters.AddWithValue("@comment", comment);

                    int affected = await command.ExecuteNonQueryAsync();

                    if (affected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return Ok();
        }


        [HttpGet("sendInfo/{addressId}")]
        public async Task<ActionResult> GetRequestsByAddress(int addressId)
        {
            List<object> result = new List<object>();

            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                string sql = "SELECT " +
                             "cr.id, " +
                             "cr.status_id, " +
                             "s.name AS status, " +
                             "ct.name AS category, " +
                             "cr.created_time, " +
                             "cr.planned_completion_time, " +
                             "cr.work_comment " +
                             "FROM clients_requests cr " +
                             "JOIN apartments ap ON ap.id = cr.apartment_id " +
                             "LEFT JOIN statuses s ON s.id = cr.status_id " +
                             "LEFT JOIN categories ct ON ct.id = cr.category_id " +
                             "WHERE ap.address_id = @address_id " +
                             "ORDER BY cr.created_time DESC";

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@address_id", addressId);

                    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new
                            {
                                id = reader.GetInt32(0),
                                status_id = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                status = reader.IsDBNull(2) ? null : reader[2].ToString(),
                                category = reader.IsDBNull(3) ? null : reader[3].ToString(),
                                created_time = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                planned_completion_time = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                work_comment = reader.IsDBNull(6) ? null : reader[6].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateClientRequestAsync([FromBody] CreateClientRequestDto data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Phone) || data.AddressId == null)
            {
                return BadRequest(new { message = "Укажите телефон и полный адрес." });
            }

            string? lastName = string.IsNullOrWhiteSpace(data.LastName) ? null : data.LastName.Trim();
            string? firstName = string.IsNullOrWhiteSpace(data.FirstName) ? null : data.FirstName.Trim();
            string? middleName = string.IsNullOrWhiteSpace(data.MiddleName) ? null : data.MiddleName.Trim();

            string phone = data.Phone.Trim();
            string? email = string.IsNullOrWhiteSpace(data.Email) ? null : data.Email.Trim();

            string apartmentNumber = string.IsNullOrWhiteSpace(data.Apartment) ? "б/н" : data.Apartment.Trim();
            if (apartmentNumber.Length > 10)
            {
                return BadRequest(new
                {
                    message = "Номер квартиры не должен превышать 10 символов."
                });
            }

            string? description = string.IsNullOrWhiteSpace(data.Comment) ? null : data.Comment.Trim();

            string notificationType = data.NotificationType == "email" ? "email" : "none";

            if (notificationType == "email" && email == null)
            {
                return BadRequest(new { message = "Укажите email для уведомления." });
            }

            string? categoryName = string.IsNullOrWhiteSpace(data.Category) ? null : data.Category.Trim();

            if (categoryName == null)
            {
                return BadRequest(new { message = "Выберите категорию обращения." });
            }



            await using (NpgsqlConnection con = _connection.getConnection())
            {
                await con.OpenAsync();

                await using (NpgsqlTransaction transaction = await con.BeginTransactionAsync())
                {
                    try
                    {
                        int clientId = 0;

                        string findClientSql = "SELECT id FROM clients " +
                                               "WHERE regexp_replace(phone, '[^0-9]', '', 'g') = " +
                                               "regexp_replace(@phone, '[^0-9]', '', 'g') " +
                                               "LIMIT 1";

                        await using (NpgsqlCommand command = new NpgsqlCommand(findClientSql, con, transaction))
                        {
                            command.Parameters.AddWithValue("@phone", phone);

                            object? result = await command.ExecuteScalarAsync();

                            if (result != null && result != DBNull.Value)
                            {
                                clientId = Convert.ToInt32(result);
                            }
                        }

                        if (clientId == 0)
                        {
                            string insertClientSql = "INSERT INTO clients " +
                                                     "(first_name, last_name, middle_name, phone, email) " +
                                                     "VALUES " +
                                                     "(@first_name, @last_name, @middle_name, @phone, @email) " +
                                                     "ON CONFLICT DO NOTHING " +
                                                     "RETURNING id";

                            await using (NpgsqlCommand command = new NpgsqlCommand(insertClientSql, con, transaction))
                            {
                                command.Parameters.AddWithValue("@first_name", (object?)firstName ?? DBNull.Value);
                                command.Parameters.AddWithValue("@last_name", (object?)lastName ?? DBNull.Value);
                                command.Parameters.AddWithValue("@middle_name", (object?)middleName ?? DBNull.Value);
                                command.Parameters.AddWithValue("@phone", phone);
                                command.Parameters.AddWithValue("@email", (object?)email ?? DBNull.Value);

                                object? result = await command.ExecuteScalarAsync();

                                if (result != null && result != DBNull.Value)
                                {
                                    clientId = Convert.ToInt32(result);
                                }
                            }

                            if (clientId == 0)
                            {
                                await using (NpgsqlCommand command = new NpgsqlCommand(findClientSql, con, transaction))
                                {
                                    command.Parameters.AddWithValue("@phone", phone);

                                    object? result = await command.ExecuteScalarAsync();

                                    if (result != null && result != DBNull.Value)
                                    {
                                        clientId = Convert.ToInt32(result);
                                    }
                                }
                            }
                        }

                        if (clientId == 0)
                        {
                            await transaction.RollbackAsync();

                            return StatusCode(500, new { message = "Не удалось определить клиента." });
                        }

                        int addressId = data.AddressId.Value;
                        int? addressOrganizationId = null;

                        string addressSql = "SELECT organization_id " +
                                            "FROM addresses " +
                                            "WHERE id = @id " +
                                            "LIMIT 1";

                        await using (NpgsqlCommand command = new NpgsqlCommand(addressSql, con, transaction))
                        {
                            command.Parameters.AddWithValue("@id", addressId);

                            object? result = await command.ExecuteScalarAsync();

                            if (result != null && result != DBNull.Value)
                            {
                                addressOrganizationId = Convert.ToInt32(result);
                            }
                        }

                        if (addressOrganizationId == null)
                        {
                            await transaction.RollbackAsync();

                            return BadRequest(new
                            {
                                message = "Для адреса не определена управляющая компания."
                            });
                        }


                        int apartmentId = 0;

                        string apartmentSql = "SELECT id FROM apartments " +
                                              "WHERE address_id = @address_id " +
                                              "AND number_apartment = @number_apartment " +
                                              "LIMIT 1";

                        await using (NpgsqlCommand command = new NpgsqlCommand(apartmentSql, con, transaction))
                        {
                            command.Parameters.AddWithValue("@address_id", addressId);
                            command.Parameters.AddWithValue("@number_apartment", apartmentNumber);

                            object? result = await command.ExecuteScalarAsync();

                            if (result != null && result != DBNull.Value)
                            {
                                apartmentId = Convert.ToInt32(result);
                            }
                        }

                        if (apartmentId == 0)
                        {
                            string insertApartmentSql = "INSERT INTO apartments " +
                                                        "(address_id, number_apartment) " +
                                                        "VALUES " +
                                                        "(@address_id, @number_apartment) " +
                                                        "ON CONFLICT (address_id, number_apartment) DO NOTHING " +
                                                        "RETURNING id";

                            await using (NpgsqlCommand command = new NpgsqlCommand(insertApartmentSql, con, transaction))
                            {
                                command.Parameters.AddWithValue("@address_id", addressId);
                                command.Parameters.AddWithValue("@number_apartment", apartmentNumber);

                                object? result = await command.ExecuteScalarAsync();

                                if (result != null && result != DBNull.Value)
                                {
                                    apartmentId = Convert.ToInt32(result);
                                }
                            }

                            if (apartmentId == 0)
                            {
                                await using (NpgsqlCommand command = new NpgsqlCommand(apartmentSql, con, transaction))
                                {
                                    command.Parameters.AddWithValue("@address_id", addressId);
                                    command.Parameters.AddWithValue("@number_apartment", apartmentNumber);

                                    object? result = await command.ExecuteScalarAsync();

                                    if (result != null && result != DBNull.Value)
                                    {
                                        apartmentId = Convert.ToInt32(result);
                                    }
                                }
                            }
                        }

                        int? reasonId = null;
                        string? reasonName = null;

                        if (data.Reason == "hot")
                        {
                            reasonName = "Обращение по качеству горячей воды";
                        }
                        else if (data.Reason == "heat")
                        {
                            reasonName = "Обращение по качеству отопления";
                        }

                        if (reasonName != null)
                        {
                            string reasonSql = "SELECT id FROM reasons " +
                                               "WHERE name = @name " +
                                               "LIMIT 1";

                            await using (NpgsqlCommand command = new NpgsqlCommand(reasonSql, con, transaction))
                            {
                                command.Parameters.AddWithValue("@name", reasonName);

                                object? result = await command.ExecuteScalarAsync();

                                if (result != null && result != DBNull.Value)
                                {
                                    reasonId = Convert.ToInt32(result);
                                }
                            }
                        }

                        int? categoryId = null;

                        if (reasonId != null)
                        {
                            string categorySql = "SELECT id FROM categories " +
                                                 "WHERE reason_id = @reason_id " +
                                                 "AND name = @name " +
                                                 "LIMIT 1";

                            await using (NpgsqlCommand command = new NpgsqlCommand(categorySql, con, transaction))
                            {
                                command.Parameters.AddWithValue("@reason_id", reasonId.Value);
                                command.Parameters.AddWithValue("@name", categoryName);

                                object? result = await command.ExecuteScalarAsync();

                                if (result != null && result != DBNull.Value)
                                {
                                    categoryId = Convert.ToInt32(result);
                                }
                            }
                        }

                        if (categoryId == null)
                        {
                            await transaction.RollbackAsync();

                            return BadRequest(new
                            {
                                message = "Не удалось определить категорию обращения."
                            });
                        }

                        int? regularityId = null;

                        if (!string.IsNullOrWhiteSpace(data.Regularity))
                        {
                            string regularitySql = "SELECT id FROM regularity " +
                                                   "WHERE name = @name " +
                                                   "LIMIT 1";

                            await using (NpgsqlCommand command = new NpgsqlCommand(regularitySql, con, transaction))
                            {
                                command.Parameters.AddWithValue("@name", data.Regularity.Trim());

                                object? result = await command.ExecuteScalarAsync();

                                if (result != null && result != DBNull.Value)
                                {
                                    regularityId = Convert.ToInt32(result);
                                }
                            }

                            if (regularityId == null)
                            {
                                await transaction.RollbackAsync();

                                return BadRequest(new
                                {
                                    message = "Не удалось определить длительность."
                                });
                            }
                        }

                        int requestId;

                        string requestSql = "INSERT INTO clients_requests " +
                                            "(client_id, apartment_id, category_id, regularity_id, " +
                                            "client_description, status_id, priority_id, dispatcher_id, " +
                                            "organization_id, notification_type) " +
                                            "VALUES " +
                                            "(@client_id, @apartment_id, @category_id, @regularity_id, " +
                                            "@client_description, 1, NULL, NULL, " +
                                            "@organization_id, @notification_type) " +
                                            "RETURNING id";

                        await using (NpgsqlCommand command = new NpgsqlCommand(requestSql, con, transaction))
                        {
                            command.Parameters.AddWithValue("@client_id", clientId);
                            command.Parameters.AddWithValue("@apartment_id", apartmentId);
                            command.Parameters.AddWithValue("@category_id", categoryId.Value);
                            command.Parameters.AddWithValue("@regularity_id", (object?)regularityId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@client_description", (object?)description ?? DBNull.Value);
                            command.Parameters.AddWithValue("@organization_id", addressOrganizationId.Value);
                            command.Parameters.AddWithValue("@notification_type", notificationType);

                            requestId = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        await transaction.CommitAsync();

                        return Ok(new
                        {
                            id = requestId,
                            number = "ТСС-" + DateTime.Now.Year + "-" + requestId.ToString("D6"),
                            status_id = 1,
                            status = "Новая"
                        });
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}
