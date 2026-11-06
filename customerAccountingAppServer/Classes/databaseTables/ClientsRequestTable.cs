namespace customerAccountingAppServer.Classes.databaseTables
{
    public class ClientsRequestTable
    {
        public int Id { get; set; }

        public string? Fio { get; set; }

        public int? Address_id { get; set; }

        public string? Address { get; set; }

        public string? Reason { get; set; }

        public string? Category { get; set; }

        public string? Duration { get; set; }

        public string? Client_description { get; set; }

        public int? Status_id { get; set; }

        public string? Status { get; set; }

        public int? Priority_id { get; set; }

        public string? Priority { get; set; }

        public int? Dispatcher_id { get; set; }

        public string? Dispatcher { get; set; }

        public string? Dispatcher_description { get; set; }

        public int? Organization_id { get; set; }

        public string? Organization { get; set; }

        public int? Employee_id { get; set; }

        public string? Employee { get; set; }

        public string? Employee_comment { get; set; }

        public string? Organization_Comment { get; set; }

        public DateTime? Created_time { get; set; }

        public DateTime? Start_time { get; set; }

        public DateTime? Planned_time { get; set; }

        public DateTime? Closed_time { get; set; }

        public int? Uk_decision { get; set; }

    }
}
