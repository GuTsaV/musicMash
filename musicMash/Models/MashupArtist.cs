using System.Collections.Generic;

namespace musicMash.Models
{
    public class MashupArtist
    {
        public MashupArtist(string id, string name, string description, List<MashupAlbum> albums)
        {
            Mbid = id;
            Name = name;
            Description = description;
            Albums = albums;
        }

        public string Mbid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MashupAlbum> Albums { get; set; }
    }
}
