using System.Collections.Generic;
using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class MusicBrainzResult : IResult
    {
        public MusicBrainzResult(string id, string name, List<MusicBrainzAlbum> albums, List<MusicBrainzRelation> relations)
        {
            Id = id;
            Name = name;
            Albums = albums;
            Relations = relations;
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "release-groups")]
        public List<MusicBrainzAlbum> Albums { get; set; }

        [DataMember(Name = "relations")]
        public List<MusicBrainzRelation> Relations { get; set; }

        public string Url { get; set; }
    }
}
