using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    public class DispatchersTable
    {
        public int Id { get; private set; }

        public int Employee_id { get; private set; }

        public string? Login { get; private set; }

        public string? Password { get; private set; }

        public DispatchersTable(int id, int employee_id, string? login, string? password)
        {
            Id = id;
            Employee_id = employee_id;
            Login = login;
            Password = password;
        }
    }
}
