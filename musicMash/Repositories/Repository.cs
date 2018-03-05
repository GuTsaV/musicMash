using System.Net.Http;
using System.Threading.Tasks;
using musicMash.Models;
using musicMash.Services;

namespace musicMash.Repositories
{
    public class Repository<T> : IRepository<T> where T : IResult
    {
        readonly IHttpHandler _httpClient;

        public Repository(IHttpHandler httpHandler)
        {
            _httpClient = httpHandler;
        }

        public async Task<T> Get(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return default(T);
                }
                var result = await response.Content.ReadAsAsync<T>();
                result.Url = url;
                return result;
            }
            catch (HttpRequestException)
            {
                return default(T);
            }
        }
    }
}
