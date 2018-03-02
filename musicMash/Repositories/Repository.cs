using System.Net.Http;
using System.Threading.Tasks;
using musicMash.Services;

namespace musicMash.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
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
                var result = response.Content.ReadAsAsync<T>();
                return await result;
            }
            catch (HttpRequestException)
            {
                return default(T);
            }
        }
    }
}
