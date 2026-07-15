using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes.databaseTables
{
    public class clientsRequestTable
    {
        public int id {  get; set; }

        public string fio { get; set; }

        public string address { get; set; }

        public string? description { get; set; }

        public string? status { get; set; }

        public string? priority { get; set; }

        public string? manager { get; set; }

        public string? master { get; set; }

        public DateTime? created_time { get; set; }

        public DateTime? visit_time { get; set; }

        public DateTime? closed_time { get; set; }

        public string? master_comment { get; set; }
    }
}
