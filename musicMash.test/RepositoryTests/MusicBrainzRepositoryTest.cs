using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;
using musicMash.Repositories;
using musicMash.Services;
using musicMash.Models;

namespace musicMash.test.RepositoryTests
{
    public class MusicBrainsRepositoryTest
    {
        Repository<MusicBrainzResult> _repository;

        public MusicBrainsRepositoryTest()
        {
            const string jsonString =
    "{\"id\": \"artist id\", \"name\": \"Nirvana\",\"release-groups\": [ {\"title\": \"Album 1\",\"id\": \"01cf1391-141b-3c87-8650-45ade6e59070\"},{\"title\": \"Album 2\",\"id\": \"01cf1391-141b-3c87-8650-45ade6e59072\"}], \"relations\": [{\"type\": \"wikipedia\",\"url\": {\"id\": \"d99c5574-096b-45af-bf20-c3dc3e94fde5\",\"resource\": \"https://en.wikipedia.org/wiki/Nirvana_(band)\"}}]}";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json") };
            var mockHttp = new Mock<IHttpHandler>();
            mockHttp.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(response);
            _repository = new Repository<MusicBrainzResult>(mockHttp.Object);
        }

        [Fact]
        public async Task GetArtistShouldReturnArtist()
        {
            var result = await _repository.Get("artist id");

            Assert.Equal("Nirvana", result.Name);
            Assert.Equal("artist id", result.Id);
        }

        [Fact]
        public async Task GetArtistShouldReturnArtistAlbums()
        {
            var result = await _repository.Get("artist id");

            Assert.Equal(2, result.Albums.Count);
            Assert.Contains(result.Albums, a => a.Title == "Album 1");
            Assert.Contains(result.Albums, a => a.Title == "Album 2");
        }

        [Fact]
        public async Task GetArtistShouldReturnRelations()
        {
            var result = await _repository.Get("artist id");

            Assert.True(result.Relations.Count > 0);
            Assert.Equal("wikipedia", result.Relations.First().Type);
        }
    }
}