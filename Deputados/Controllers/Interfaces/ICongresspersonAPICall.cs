using Congressperson.HttpClients.Interfaces;
using Congressperson.Models.DTO;
using System.Threading.Tasks;

namespace Congressperson.Controllers.Interfaces
{
    public interface ICongresspersonAPICall
    {
        public IHttpClients Client { get; set; }
        Task<DTOCongressperson> GetCongresspeopleAsync();
    }
}
