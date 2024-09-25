namespace MusicStore.Models
{
	public class CartLine
    {
        public int Id { get; set; }
        public Album Album { get; set; }
        public int Quantity { get; set; }
    }
}
