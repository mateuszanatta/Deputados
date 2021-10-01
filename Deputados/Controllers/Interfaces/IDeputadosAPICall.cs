using Deputados.HttpClients.Interfaces;
using Deputados.Models;
using System.Threading.Tasks;

namespace Deputados.Controllers.Interfaces
{
    public interface IDeputadosAPICall
    {
        public IHttpClients Client { get; set; }
        Task<Dados> GetDadosAsync();
    }
}
