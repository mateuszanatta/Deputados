namespace Congressperson.Models
{
    public interface ICongresspersonDatabaseSettings
    {
        string CongresspersonCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class CongresspersonDatabaseSettings : ICongresspersonDatabaseSettings
    {
        public string CongresspersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
