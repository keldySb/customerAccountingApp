namespace finalExam_diplom_.classes.databaseTables
{
    public class DictionaryItems
    {
        public int Id { get; private set; }

        public string? Name { get; private set; }

        public DictionaryItems(int id, string? name)
        {
            Id = id; 
            Name = name;
        }
    }
}
