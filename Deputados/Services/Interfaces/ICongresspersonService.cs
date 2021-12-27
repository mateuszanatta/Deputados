
using Congressperson.Models;
using Congressperson.Models.DTO;
using System.Collections.Generic;

namespace Congressperson.Services.Interfaces
{
    public interface ICongresspersonService
    {
        IEnumerable<Models.Congressperson> Get();
        Models.Congressperson Get(string id);
        Models.Congressperson GetByIdCongressperson(int idCongressperson);
        CongresspersonStatistics GetCongresspersonStatistics(int idCongressperson);
        void Insert(Models.Congressperson congressperson);
        void InsertMany(IEnumerable<Models.Congressperson> congresspeople);
        void Update(Models.Congressperson congressperson);
        void Remove(Models.Congressperson congressperson);
        void Remove(string id);
    }
}
