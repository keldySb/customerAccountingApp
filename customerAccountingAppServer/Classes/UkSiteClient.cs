using Npgsql;

namespace customerAccountingAppServer.Classes
{
    public class UkSiteClient
    {
        private readonly DataBaseConnection _connection;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UkSiteClient> _logger;

        public UkSiteClient(
            DataBaseConnection connection,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<UkSiteClient> logger)
        {
            _connection = connection;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendRequestAsync(int requestId)
        {
            try
            {
                UkSiteTarget? target = await GetTargetAsync(requestId);

                if (target == null)
                {
                    return;
                }

                string? baseUrl = _configuration[$"UkSites:{target.OrganizationId}"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    _logger.LogInformation(
                        "Для организации {OrganizationId} не настроен UkSites.",
                        target.OrganizationId);

                    return;
                }

                if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? baseUri) ||
                    baseUri.Scheme != Uri.UriSchemeHttp &&
                     baseUri.Scheme != Uri.UriSchemeHttps)
                {
                    _logger.LogWarning(
                        "Некорректный адрес сайта УК для организации {OrganizationId}: {Url}",
                        target.OrganizationId,
                        baseUrl);

                    return;
                }

                var payload = new
                {
                    sourceRequestId = requestId,
                    organizationId = target.OrganizationId,
                    organizationName = target.OrganizationName ?? "",
                    address = target.Address ?? "",
                    apartment = target.Apartment ?? "",
                    clientName = target.ClientName ?? "",
                    phone = target.Phone ?? "",
                    reason = target.Reason ?? "",
                    category = target.Category ?? "",
                    regularity = target.Regularity ?? "",
                    clientDescription = target.ClientDescription ?? "",
                    createdTime = target.CreatedTime
                };

                HttpClient client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(10);

                string url = $"{baseUri.ToString().TrimEnd('/')}/api/uk/requests";

                HttpResponseMessage response = await client.PostAsJsonAsync(url, payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Сайт УК вернул {StatusCode} для заявки {RequestId}.",
                        (int)response.StatusCode,
                        requestId);

                    return;
                }

                _logger.LogInformation(
                    "Заявка {RequestId} отправлена на сайт УК {Url}.",
                    requestId,
                    url);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Не удалось отправить заявку {RequestId} на сайт УК.",
                    requestId);
            }
        }

        private async Task<UkSiteTarget?> GetTargetAsync(int requestId)
        {
            const string sql = """
                SELECT
                    COALESCE(cr.organization_id, a.organization_id) AS organization_id,
                    o.name AS organization_name,
                    CONCAT('г. ', a.city, ', ул. ', a.street, ', д. ', a.house_number) AS address,
                    ap.number_apartment,
                    CONCAT_WS(' ', c.last_name, c.first_name, c.middle_name) AS client_name,
                    c.phone,
                    r.name AS reason,
                    ct.name AS category,
                    reg.name AS regularity,
                    cr.client_description,
                    cr.created_time
                FROM clients_requests cr
                LEFT JOIN apartments ap ON ap.id = cr.apartment_id
                LEFT JOIN addresses a ON a.id = ap.address_id
                LEFT JOIN organizations o ON o.id = COALESCE(cr.organization_id, a.organization_id)
                LEFT JOIN clients c ON c.id = cr.client_id
                LEFT JOIN categories ct ON ct.id = cr.category_id
                LEFT JOIN reasons r ON r.id = ct.reason_id
                LEFT JOIN regularity reg ON reg.id = cr.regularity_id
                WHERE cr.id = @id
                LIMIT 1
                """;

            await using NpgsqlConnection connection = _connection.getConnection();
            await connection.OpenAsync();

            await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", requestId);

            await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync() || reader.IsDBNull(0))
            {
                return null;
            }

            return new UkSiteTarget(
                reader.GetInt32(0),
                reader.IsDBNull(1) ? null : reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.IsDBNull(6) ? null : reader.GetString(6),
                reader.IsDBNull(7) ? null : reader.GetString(7),
                reader.IsDBNull(8) ? null : reader.GetString(8),
                reader.IsDBNull(9) ? null : reader.GetString(9),
                reader.IsDBNull(10) ? null : reader.GetDateTime(10));
        }

        private sealed record UkSiteTarget(
            int OrganizationId,
            string? OrganizationName,
            string? Address,
            string? Apartment,
            string? ClientName,
            string? Phone,
            string? Reason,
            string? Category,
            string? Regularity,
            string? ClientDescription,
            DateTime? CreatedTime);
    }
}
