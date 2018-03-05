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
    public class WikipediaRepositoryTest
    {
        Repository<WikipediaResult> _repository;

        public WikipediaRepositoryTest()
        {
            const string jsonString = "{\"query\":{\"pages\":{\"123\":{\"title\":\"Nirvana (band)\",\"extract\":\"<p><b>Nirvana</b> was an American rock band formed by singer and guitarist Kurt Cobain and bassist Krist Novoselic in Aberdeen, Washington, in 1987. Nirvana went through a succession of drummers, the longest-lasting being Dave Grohl, who joined in 1990.\"}}}}";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json") };
            var mockHttp = new Mock<IHttpHandler>();
            mockHttp.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(response);
            _repository = new Repository<WikipediaResult>(mockHttp.Object);
        }

        [Fact]
        public async Task GetPageShouldReturnExtract()
        {
            var result = await _repository.Get("Nirvana_(band)");

            var firstKey = result.Query.Pages.First().Key;
            var description = result.Query.Pages[firstKey].Description;
            Assert.Contains("<p><b>Nirvana</b> was an American rock band formed by singer and guitarist Kurt Cobain", description);
        }
    }
}
