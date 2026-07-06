using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes
{
    class dataBaseConnection
    {

        NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=uk_application_db");


        private void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public NpgsqlConnection getConnection()
        {
            return connection;
        }
    }
}
