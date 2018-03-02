using System.Collections.Generic;
using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class CoverArtResult
    {
        public CoverArtResult(List<CoverArtImage> images, string albumId)
        {
            Images = images;
            AlbumId = albumId;
        }

        [DataMember(Name = "images")]
        public List<CoverArtImage> Images { get; set; }
        public string AlbumId { get; set; }
    }
}
