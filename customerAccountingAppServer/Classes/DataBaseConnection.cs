using Npgsql;

namespace customerAccountingAppServer.Classes
{
    public class DataBaseConnection
    {
        private readonly string _connectionString;

        public DataBaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException(
                    "Не удалось подключиться к базе данных");
        }

        public NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
