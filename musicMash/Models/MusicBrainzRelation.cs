using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class MusicBrainzRelation
    {
        public MusicBrainzRelation(string type, MusicBrainzUrl url)
        {
            Type = type;
            Url = url;
        }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "url")]
        public MusicBrainzUrl Url { get; set; }
    }
}
