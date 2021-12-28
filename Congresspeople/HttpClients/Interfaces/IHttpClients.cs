using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Congressperson.HttpClients.Interfaces
{
    public interface IHttpClients: IDisposable
    {
        void Clear();
        void Add(string mediaType);
        Task<Stream> GetStreamAsync(string requestUri);
        Task<HttpResponseMessage> GetAsync(string? requestUri);
    }
}
