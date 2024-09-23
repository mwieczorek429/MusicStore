namespace MusicStore.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
