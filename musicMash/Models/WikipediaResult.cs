using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class WikipediaResult : IResult
    {
        public WikipediaResult(WikipediaQuery query)
        {
            Query = query;
        }

        [DataMember(Name = "query")]
        public WikipediaQuery Query { get; set; }

        public string Url { get; set; }
    }
}
