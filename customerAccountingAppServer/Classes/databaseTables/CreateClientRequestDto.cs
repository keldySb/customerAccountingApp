namespace customerAccountingAppServer.Classes.databaseTables
{
    public class CreateClientRequestDto
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public int? AddressId { get; set; }
        public string? Apartment { get; set; }

        public string? Reason { get; set; }
        public string? Category { get; set; }
        public string? Regularity { get; set; }
        public string? Comment { get; set; }

        public string? NotificationType { get; set; }

    }
}
