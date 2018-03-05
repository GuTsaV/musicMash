using System.Collections.Generic;
using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class CoverArtResult : IResult
    {
        public CoverArtResult(List<CoverArtImage> images, string url)
        {
            Images = images;
            Url = url;
        }

        [DataMember(Name = "images")]
        public List<CoverArtImage> Images { get; set; }

        public string Url { get; set; }
    }
}
