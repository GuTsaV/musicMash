using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class MusicBrainzAlbum
    {
        public MusicBrainzAlbum(string id, string title)
        {
            Id = id;
            Title = title;
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
