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

        public string? employee_number { get; set; }

        public string? fio { get; set; }

        public int age { get; set; }

        public string? position { get; set; }

        public int experience { get; set; }

        public string? phone_number { get; set; }

        public string? status { get; set; }
    }
}
