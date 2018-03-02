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
    public class CoverArtRepositoryTest
    {
        private Repository<CoverArtResult> _repository;

        public CoverArtRepositoryTest()
        {
            const string jsonString = "{\"images\":[{\"image\": \"http://lank.till.bild/hej.jpg\"}]}";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json") };
            var mockHttp = new Mock<IHttpHandler>();
            mockHttp.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(response);
            _repository = new Repository<CoverArtResult>(mockHttp.Object);
        }

        [Fact]
        public async Task GetAlbumShouldReturnImage()
        {
            var result = await _repository.Get("album id");

            Assert.Equal("http://lank.till.bild/hej.jpg", result.Images.First().ImageUrl);
        }
    }
}
