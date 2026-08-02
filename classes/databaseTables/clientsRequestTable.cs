namespace finalExam_diplom_.classes.databaseTables
{
    public class ClientsRequestTable
    {
        public int Id { get; private set; }

        public string? Fio { get; private set; }

        public string? Address { get; private set; }

        public string? Description { get; private set; }

        public int? Status_id { get; private set; }

        public string? Status { get; private set; }

        public int Priority_id { get; private set; }

        public string? Priority { get; private set; }

        public int? Manager_id { get; private set; }

        public string? Manager { get; private set; }

        public int? Master_id { get; private set; }

        public string? Master { get; private set; }

        public DateTime? Created_time { get; private set; }

        public DateTime? Visit_time { get; private set; }

        public DateTime? Closed_time { get; private set; }

        public string? Master_comment { get; private set; }

        public ClientsRequestTable(int id, string? fio, string? address, string? description, int? status_id, string? status, int priority_id, string? priority, int? manager_id, string? manager, int? master_id,
    string? master, DateTime? created_time, DateTime? visit_time, DateTime? closed_time, string? master_comment)
        {
            Id = id;
            Fio = fio;
            Address = address;
            Description = description;
            Status_id = status_id;
            Status = status;
            Priority_id = priority_id;
            Priority = priority;
            Manager_id = manager_id;
            Manager = manager;
            Master_id = master_id;
            Master = master;
            Created_time = created_time;
            Visit_time = visit_time;
            Closed_time = closed_time;
            Master_comment = master_comment;
        }
    }
} 
