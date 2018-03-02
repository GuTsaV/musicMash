using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class CoverArtImage
    {
        public CoverArtImage(string url)
        {
            ImageUrl = url;
        }

        [DataMember(Name = "image")]
        public string ImageUrl { get; set; }
    }
}
