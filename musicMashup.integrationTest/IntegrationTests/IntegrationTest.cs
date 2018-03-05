using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using musicMash.Controllers;
using musicMash.Models;
using musicMash.Repositories;
using musicMash.Services;
using Xunit;

namespace musicMashup.integrationTest.IntegrationTests
{
    public class IntegrationTest
    {
        MashupController _controller;
        IHttpHandler _httpHandler;
        IMashupService _service;
        IRepository<CoverArtResult> _coverArtRepository;
        IRepository<MusicBrainzResult> _musicBrainzRepository;
        IRepository<WikipediaResult> _wikipediaRepository;

        public IntegrationTest()
        {
            _httpHandler = new HttpClientHandler();
            _coverArtRepository = new Repository<CoverArtResult>(_httpHandler);
            _musicBrainzRepository = new Repository<MusicBrainzResult>(_httpHandler);
            _wikipediaRepository = new Repository<WikipediaResult>(_httpHandler);
            _service = new MashupService(_coverArtRepository, _musicBrainzRepository, _wikipediaRepository);
            _controller = new MashupController(_service);
        }


        [Fact]
        public async Task GetMashupForQueenShouldReturnQueen()
        {
            var response = await _controller.Get("0383dadf-2a4e-4d10-a46a-e9e041da8eb3");

            var okResult = Assert.IsType<OkObjectResult>(response);
            var mashup = Assert.IsType<MashupArtist>(okResult.Value);
            Assert.Equal("Queen", mashup.Name);
            Assert.Equal(25, mashup.Albums.Count);
            Assert.Contains("<p><b>Queen</b> are a British rock band that formed in London in 1970.", mashup.Description);
        }
    }
}
