using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    internal class addressesTable
    {
        public int id { get; set; }

        public static string street { get; set; }

        public static string house_number { get; set; }

        public int entrance { get; set; }

        public int floor { get; set; }

        public static string number_apartment { get; set; }
    }
}
