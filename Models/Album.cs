using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, ErrorMessage = "Author cannot be longer than 100 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters.")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "Release Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100.")]
        public int ReleaseYear { get; set; }
        public TimeSpan? Duration { get; set; }
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string? Description { get; set; }
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ImageUrl { get; set; }
    }

}
