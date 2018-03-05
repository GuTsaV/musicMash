using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using musicMash.Exceptions;
using musicMash.Models;
using musicMash.Repositories;

namespace musicMash.Services
{
    public class MashupService : IMashupService
    {
        readonly IRepository<CoverArtResult> _coverArtRepository;
        readonly IRepository<MusicBrainzResult> _musicBrainzRepository;
        readonly IRepository<WikipediaResult> _wikipediaRepository;
        // Take everything from the end of the string that is not a '/'
        const string PageNamePattern = @"[^\/]*$";
        static Regex Regexp = new Regex(PageNamePattern);

        public MashupService(
            IRepository<CoverArtResult> coverArtRepository,
            IRepository<MusicBrainzResult> musicBrainzRepository,
            IRepository<WikipediaResult> wikipediaRepository
        )
        {
            _coverArtRepository = coverArtRepository;
            _musicBrainzRepository = musicBrainzRepository;
            _wikipediaRepository = wikipediaRepository;
        }

        public async Task<MashupArtist> GetMashup(string artistId)
        {
            // Fetch info from MusizBrainz first
            var musicBrainzResult = await GetArtist(Configuration.MusicBrainzArtistUrl(artistId));

            var albums = musicBrainzResult.Albums.Select(musicBrainzAlbum => new MashupAlbum(musicBrainzAlbum.Id, musicBrainzAlbum.Title)).ToList();

            // Set up other tasks
            var wikipediaTask = GetWikipedia(musicBrainzResult);
            var coverTasks = albums.Select(album => _coverArtRepository.Get(Configuration.CoverArtAlbumUrl(album.Id))).ToList();

            // Wikipedia and Cover Art Archive are fetched concurrently
            // Wait for Wikipedia and collect result
            var wikipediaResult = await wikipediaTask;
            var firstKey = wikipediaResult.Query?.Pages?.First().Key;
            var description = wikipediaResult.Query?.Pages[firstKey]?.Description;

            // Wait for Cover Art Archive and collect results
            await Task.WhenAll(coverTasks);
            albums = HandleAlbums(coverTasks, albums);

            return new MashupArtist(artistId, musicBrainzResult.Name, description, albums);
        }

        async Task<MusicBrainzResult> GetArtist(string url)
        {
            var musicBrainzResult = await _musicBrainzRepository.Get(url);
            if (musicBrainzResult == null)
            {
                throw new ArtistDoesNotExistException();
            }
            return musicBrainzResult;
        }

        Task<WikipediaResult> GetWikipedia(MusicBrainzResult musicBrainzResult)
        {
            var wikipediaRelation = musicBrainzResult.Relations?.FirstOrDefault(r => r.Type == "wikipedia");
            if (wikipediaRelation == null)
            {
                throw new InvalidOperationException("Missing Wikipedia relation");
            }
            var wikipediaPageName = GetWikipediaPageName(wikipediaRelation.Url.Resource);
            var wikipediaUrl = Configuration.WikipediaPageUrl(wikipediaPageName);
            return _wikipediaRepository.Get(wikipediaUrl);
        }

        List<MashupAlbum> HandleAlbums(List<Task<CoverArtResult>> coverTasks, List<MashupAlbum> albums)
        {
            var coverArtAlbums = new List<CoverArtResult>();
            coverTasks.ForEach(task =>
            {
                if (task.Result?.Images != null)
                {
                    coverArtAlbums.Add(task.Result);
                }
            });

            albums.ForEach(album =>
            {
                var cover = coverArtAlbums.FirstOrDefault(x => x.Url.Contains(album.Id));
                if (cover?.Images == null)
                {
                    album.Image = "Not found";
                }
                else
                {
                    album.Image = cover.Images.First().ImageUrl;
                }
            });
            return albums;
        }

        static string GetWikipediaPageName(string wikipediaUrl)
        {
            var matches = Regexp.Match(wikipediaUrl);
            return matches.Success ? matches.Value : null;
        }
    }
}
