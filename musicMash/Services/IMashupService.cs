using System.Threading.Tasks;
using musicMash.Models;

namespace musicMash.Services
{
    public interface IMashupService
    {
        Task<MashupArtist> GetMashup(string artistId);
    }
}
