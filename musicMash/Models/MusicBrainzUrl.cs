using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class MusicBrainzUrl
    {
        public MusicBrainzUrl(string url)
        {
            Resource = url;
        }

        [DataMember(Name = "resource")]
        public string Resource { get; set; }
    }
}
