namespace finalExam_diplom_.classes.databaseTables
{
    public class ClientsRequestTable
    {
        public int Id { get; set; }

        public string? Fio { get; set; }

        public string? Address { get; set; }

        public string? Reason { get; set; }

        public string? Category { get; set; }

        public string? Duration { get; set; }

        public string? Description { get; set; }

        public int? Status_id { get; set; }

        public string? Status { get; set; }

        public int Priority_id { get; set; }

        public string? Priority { get; set; }

        public int? Manager_id { get; set; }

        public string? Manager { get; set; }

        public int? Master_id { get; set; }

        public string? Master { get; set; }

        public DateTime? Created_time { get; set; }

        public DateTime? Visit_time { get; set; }

        public DateTime? Closed_time { get; set; }

        public string? Master_comment { get; set; }
    }
} 
