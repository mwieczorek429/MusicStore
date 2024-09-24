namespace MusicStore.Models
{
	public class CartLine
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int Quantity { get; set; }
    }
}
