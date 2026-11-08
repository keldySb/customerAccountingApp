using Npgsql;

namespace customerAccountingAppServer.Classes
{
    public class WorkCompletionStub : BackgroundService
    {
        private readonly DataBaseConnection _connection;
        private readonly ILogger<WorkCompletionStub> _logger;

        public WorkCompletionStub(
            DataBaseConnection connection,
            ILogger<WorkCompletionStub> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(180), stoppingToken);

                    await using NpgsqlConnection con = _connection.getConnection();
                    await con.OpenAsync();

                    string sql = """
                    UPDATE clients_requests
                    SET status_id = 5,
                        closed_time = CURRENT_TIMESTAMP,
                        work_comment = 'Работы выполнены'
                    WHERE status_id = 4
                      AND uk_decision = 2
                      AND work_comment IS NULL
                    """;

                    await using NpgsqlCommand command = new NpgsqlCommand(sql, con);

                    int affected = await command.ExecuteNonQueryAsync();

                    if (affected > 0)
                    {
                        _logger.LogInformation(
                            "Завершено {Count} заявок.",
                            affected);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Ошибка выполнения работ.");
                }
            }
        }
    }
}
