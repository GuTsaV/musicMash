﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using musicMash.Models;
using musicMash.Repositories;

namespace musicMash.Services
{
    public class MashupService : IMashupService
    {
        private readonly IRepository<CoverArtResult> _coverArtRepository;
        private readonly IRepository<MusicBrainzResult> _musicBrainzRepository;
        private readonly IRepository<WikipediaResult> _wikipediaRepository;
        // Take everything from the end of the string that is not a '/'
        private const string PageNamePattern = @"[^\/]*$";

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
            var musicBrainzResult = await GetArtist(artistId);
            var albums = musicBrainzResult.Albums.Select(musicBrainzAlbum => new MashupAlbum(musicBrainzAlbum.Id, musicBrainzAlbum.Title)).ToList();

            // Set up other tasks
            var wikipediaTask = GetWikipedia(musicBrainzResult);
            var coverTasks = albums.Select(album => _coverArtRepository.Get(album.Id)).ToList();

            // Wikipedia and Cover Art Archive are fetched concurrently
            // Wait for Wikipedia and collect result
            var wikipediaResult = await wikipediaTask;
            var firstKey = wikipediaResult.Query.Pages.First().Key;
            var description = wikipediaResult.Query.Pages[firstKey].Description;

            // Wait for Cover Art Archive and collect results
            await Task.WhenAll(coverTasks);
            albums = HandleAlbums(coverTasks, albums);

            return new MashupArtist(artistId, musicBrainzResult.Name, description, albums);
        }

        private async Task<MusicBrainzResult> GetArtist(string artistId)
        {
            var musicBrainzResult = await _musicBrainzRepository.Get(artistId);
            if (musicBrainzResult == null)
            {
                throw new InvalidOperationException("No artist found");
            }
            return musicBrainzResult;
        }

        private Task<WikipediaResult> GetWikipedia(MusicBrainzResult musicBrainzResult)
        {
            var wikipediaRelation = musicBrainzResult.Relations.FirstOrDefault(r => r.Type == "wikipedia");
            if (wikipediaRelation == null)
            {
                throw new InvalidOperationException("Missing Wikipedia relation");
            }
            var wikipediaPageName = GetWikipediaPageName(wikipediaRelation.Url.Resource);
            return _wikipediaRepository.Get(wikipediaPageName);
        }

        private List<MashupAlbum> HandleAlbums(List<Task<CoverArtResult>> coverTasks, List<MashupAlbum> albums)
        {
            var coverArtAlbums = new List<CoverArtResult>();
            coverTasks.ForEach(task =>
            {
                if (task.IsCompleted && task.Result?.Images != null)
                {
                    coverArtAlbums.Add(task.Result);
                }
            });

            albums.ForEach(album =>
            {
                var cover = coverArtAlbums.FirstOrDefault(x => x.AlbumId == album.Id);
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

        private static string GetWikipediaPageName(string wikipediaUrl)
        {
            var regexp = new Regex(PageNamePattern);
            var matches = regexp.Match(wikipediaUrl);
            return string.IsNullOrWhiteSpace(matches.Value) ? null : matches.Value;
        }
    }
}