using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    class clientsRequestTable
    {
        public int id {  get; set; }

        public int client_id { get; set; }

        public int apartment_id { get; set; }

        public static string description { get; set; }

        public int status_id { get; set; }

        public int priority_id { get; set; }

        public int manager_id { get; set; }

        public int master_id { get; set; }

        public DateTime created_time { get; set; }

        public DateTime visit_time { get; set; }

        public DateTime closed_time { get; set; }

        public static string master_comment { get; set; }
    }
}
