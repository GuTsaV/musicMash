using System.Net.Http;
using System.Threading.Tasks;

namespace musicMash.Services
{
    public class HttpClientHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();

        public HttpClientHandler(){
            // Headers are needed to get the correct type of result, that is a JSON result
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }
}
