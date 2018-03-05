namespace musicMash
{
    public static class Configuration
    {
        static Configuration()
        {
            MusicBrainzUrl = Startup.Configuration["urls:musicBrainzUrl"];
            WikipediaUrl = Startup.Configuration["urls:wikipediaUrl"];
            CoverArtUrl = Startup.Configuration["urls:coverArtUrl"];
        }

        public static string MusicBrainzArtistUrl(string artistId) => $"{MusicBrainzUrl}/artist/{artistId}?fmt=json&inc=url-rels+release-groups";
        public static string WikipediaPageUrl(string pageName) => $"{WikipediaUrl}?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles={pageName}";
        public static string CoverArtAlbumUrl(string albumId) => $"{CoverArtUrl}/{albumId}";

        static readonly string MusicBrainzUrl;
        static readonly string WikipediaUrl;
        static readonly string CoverArtUrl;
    }
}
