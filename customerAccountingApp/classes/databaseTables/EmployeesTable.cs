using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    public class EmployeesTable
    {
        public int Id { get; set; }

        public string? Employee_number { get; set; }

        public string? Fio { get; set; }

        public int? Age { get; set; }

        public int? Position_Id { get; set; }

        public string? Position { get; set; }

        public int? Experience { get; set; }

        public string? Phone_number { get; set; }

        public int? Status_Id { get; set; } 

        public string? Status { get; set; }

    }
}
