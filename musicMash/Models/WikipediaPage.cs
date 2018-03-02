using System.Runtime.Serialization;

namespace musicMash.Models
{
    [DataContract]
    public class WikipediaPage
    {
        public WikipediaPage(string title, string description)
        {
            Title = title;
            Description = description;
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "extract")]
        public string Description { get; set; }
    }
}
