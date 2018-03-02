using System.Collections.Generic;
using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class WikipediaQuery
    {
        public WikipediaQuery(Dictionary<string, WikipediaPage> pages)
        {
            Pages = pages;
        }

        [DataMember(Name = "pages")]
        public Dictionary<string, WikipediaPage> Pages { get; set; }
    }
}
