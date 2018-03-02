using System.Net.Http;
using System.Threading.Tasks;

namespace musicMash.Services
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
