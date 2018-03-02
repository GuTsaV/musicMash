using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class WikipediaResult
    {
        public WikipediaResult(WikipediaQuery query)
        {
            Query = query;
        }

        [DataMember(Name = "query")]
        public WikipediaQuery Query { get; set; }
    }
}
