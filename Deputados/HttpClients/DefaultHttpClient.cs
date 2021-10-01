using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Deputados.HttpClients.Interfaces;

namespace Deputados.HttpClients
{
    public class DefaultHttpClient : IHttpClients
    {
        private static readonly HttpClient client = new();

        public void Add(string mediaType)
        {
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public void Clear()
        {
            client.DefaultRequestHeaders.Accept.Clear();
        }

        public Task<Stream> GetStreamAsync(string requestUri)
        {
            return client.GetStreamAsync(requestUri);
        }
    }
}
