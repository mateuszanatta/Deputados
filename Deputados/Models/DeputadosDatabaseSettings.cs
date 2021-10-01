namespace Deputados.Models
{
    public interface IDeputadosDatabaseSettings
    {
        string DeputadosCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class DeputadosDatabaseSettings : IDeputadosDatabaseSettings
    {
        public string DeputadosCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
