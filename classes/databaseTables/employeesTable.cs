using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    public class EmployeesTable
    {
        public int Id { get; private set; }

        public string? Employee_number { get; private set; }

        public string? Fio { get; private set; }

        public int Age { get; private set; }

        public string? Position { get; private set; }

        public int Experience { get; private set; }

        public string? Phone_number { get; private set; }

        public string? Status_Id { get; private set; } 

        public string? Status { get; private set; }


        public EmployeesTable(int id, string? employee_number, string? fio, int age, string? position, int experience, string? phone_number, string? status_id, string? status)
        {
            Id = id;
            Employee_number = employee_number;
            Fio = fio;
            Age = age;
            Position = position;
            Experience = experience;
            Phone_number = phone_number;
            Status_Id = status_id;
            Status = status;
        }
    }
}
