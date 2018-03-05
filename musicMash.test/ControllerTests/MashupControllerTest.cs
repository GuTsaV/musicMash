using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using musicMash.Controllers;
using musicMash.Models;
using musicMash.Services;
using Xunit;

namespace musicMash.test.ControllerTests
{
    public class MashupControllerTest
    {
        private MashupController _controller;

        public MashupControllerTest()
        {
            var mashupAlbums = new List<MashupAlbum> { new MashupAlbum("mbid", "Album name") };
            var mashupArtist = new MashupArtist("Artist id", "Artist name", "Description", mashupAlbums);
            var mockMusic = new Mock<IMashupService>();
            mockMusic.Setup(m => m.GetMashup(It.IsAny<string>())).ReturnsAsync(mashupArtist);

            _controller = new MashupController(mockMusic.Object);
        }

        [Fact]
        public async Task GetArtistShouldReturnArtistMashup()
        {
            var response = await _controller.Get("Artist id");

            var okResult = Assert.IsType<OkObjectResult>(response);
            var mashup = Assert.IsType<MashupArtist>(okResult.Value);

            Assert.Equal("Artist name", mashup.Name);
            Assert.Equal("Album name", mashup.Albums.First().Title);
        }
    }
}
