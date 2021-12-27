
using Congressperson.Models;
using Congressperson.Models.DTO;
using System.Collections.Generic;

namespace Congressperson.Services.Interfaces
{
    public interface ICongresspersonService
    {
        IEnumerable<Models.Congressperson> Get();
        Models.Congressperson Get(string id);
        Models.Congressperson GetByIdDeputado(int idDeputado);
        CongresspersonStatistics GetDeputadoStatistics(int idDeputado);
        void Insert(Models.Congressperson deputado);
        void InsertMany(IEnumerable<Models.Congressperson> deputados);
        void Update(Models.Congressperson deputado);
        void Remove(Models.Congressperson deputado);
        void Remove(string id);
    }
}
