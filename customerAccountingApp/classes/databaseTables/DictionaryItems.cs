namespace finalExam_diplom_.classes.databaseTables
{
    public class DictionaryItems
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DictionaryItems(int id, string? name)
        {
            Id = id; 
            Name = name;
        }
    }
}
