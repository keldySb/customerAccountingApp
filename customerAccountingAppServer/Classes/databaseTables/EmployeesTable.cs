namespace customerAccountingAppServer.Classes.databaseTables
{
    public class EmployeesTable
    {
        public int Id { get; set; }

        public string? Fio { get; set; }

        public int? Position_Id { get; set; }

        public string? Position { get; set; }

        public string? Phone_number { get; set; }

        public int Employee_status_id { get; set; }

        public string? Employee_status { get; set; }

    }
}
