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

        private readonly string connection = "Host=localhost;Username=postgres;Password=123;Database=uk_applications_db";
        public NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(connection);
        }
    }
}
