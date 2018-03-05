using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using musicMash.Models;
using musicMash.Repositories;
using musicMash.Services;
using Xunit;

namespace musicMash.test.ServiceTests
{
    public class MashupServiceTest
    {
        IMashupService _service;
        const string ArtistId = "5b11f4ce-a62d-471e-81fc-a69a8278c7da";

        public MashupServiceTest()
        {
            var coverArtImages = new List<CoverArtImage> { new CoverArtImage("https://www.en.bild.se/") };
            var coverArtResult = new CoverArtResult(coverArtImages, "album 1");
            var mockCoverArt = new Mock<IRepository<CoverArtResult>>();
            mockCoverArt.Setup(m => m.Get(It.IsAny<string>())).ReturnsAsync(coverArtResult);

            var musicBrainzAlbum = new MusicBrainzAlbum("album 1", "Album name");
            var musicBrainzRelation = new MusicBrainzRelation("wikipedia", new MusicBrainzUrl("http://en.url/Artisen"));
            var musicBrainzResult = new MusicBrainzResult(ArtistId, "Artist name",
                new List<MusicBrainzAlbum> { musicBrainzAlbum },
                new List<MusicBrainzRelation> { musicBrainzRelation });
            var mockMusicBrainz = new Mock<IRepository<MusicBrainzResult>>();
            mockMusicBrainz.Setup(m => m.Get(It.IsAny<string>())).ReturnsAsync(musicBrainzResult);

            var wikipediaPage = new WikipediaPage("Artist name", "Artist description");
            var wikipediaDictionary = new Dictionary<string, WikipediaPage>
            {
                {"Artist page id", wikipediaPage }
            };
            var wikipediaQuery = new WikipediaQuery(wikipediaDictionary);
            var wikipediaResult = new WikipediaResult(wikipediaQuery);
            var mockWikipedia = new Mock<IRepository<WikipediaResult>>();
            mockWikipedia.Setup(m => m.Get(It.IsAny<string>())).ReturnsAsync(wikipediaResult);

            _service = new MashupService(mockCoverArt.Object, mockMusicBrainz.Object, mockWikipedia.Object);
        }

        [Fact]
        public async Task GetArtistShouldReturnArtistObject()
        {
            var result = await _service.GetMashup(ArtistId);

            Assert.Equal("Artist name", result.Name);
            Assert.Equal("Artist description", result.Description);
            Assert.Equal(ArtistId, result.Mbid);
            Assert.Single(result.Albums);
            Assert.Equal("Album name", result.Albums.First().Title);
            Assert.Equal("https://www.en.bild.se/", result.Albums.First().Image);
        }
    }
}
