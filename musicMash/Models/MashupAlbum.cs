namespace musicMash.Models
{
    public class MashupAlbum
    {
        public MashupAlbum(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
