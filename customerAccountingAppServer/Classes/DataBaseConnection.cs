using Npgsql;

namespace customerAccountingAppServer.Classes
{
    class DataBaseConnection
    {
        private readonly string connection = "Host=localhost;Username=postgres;Password=123;Database=uk_applications_db";
        public NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(connection);

        }
    }
}
