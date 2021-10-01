using System.IO;
using System.Threading.Tasks;

namespace Deputados.HttpClients.Interfaces
{
    public interface IHttpClients
    {
        void Clear();
        void Add(string mediaType);
        Task<Stream> GetStreamAsync(string requestUri);
    }
}
