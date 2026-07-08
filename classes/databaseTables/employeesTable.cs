using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    public class employeesTable
    {
        public int id { get; set; }

        public static string employee_number { get; set; }

        public static string first_name { get; set; }

        public static string last_name { get; set; }

        public static string middle_name { get; set; }

        public int age { get; set; }

        public static string position { get; set; }

        public int experience { get; set; }

        public static string phone_number { get; set; }

        public int status_id { get; set; }

        public Boolean is_active { get; set; }
    }
}
