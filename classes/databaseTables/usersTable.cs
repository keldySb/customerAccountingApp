using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    internal class usersTable
    {
        public int id { get; set; }

        public int employee_id { get; set; }

        public static string login { get; set; }

        public static string password { get; set; }
    }
}
