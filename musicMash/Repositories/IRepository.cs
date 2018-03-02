using System.Threading.Tasks;

namespace musicMash.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Get(string url);
    }
}
