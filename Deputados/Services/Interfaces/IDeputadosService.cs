
using Deputados.Models;
using Deputados.Models.DTO;
using System.Collections.Generic;

namespace Deputados.Services.Interfaces
{
    public interface IDeputadosService
    {
        IEnumerable<Deputado> Get();
        Deputado Get(string id);
        Deputado GetByIdDeputado(int idDeputado);
        DeputadoStatistics GetDeputadoStatistics(int idDeputado);
        void Insert(Deputado deputado);
        void InsertMany(IEnumerable<Deputado> deputados);
        void Update(Deputado deputado);
        void Remove(Deputado deputado);
        void Remove(string id);
    }
}
