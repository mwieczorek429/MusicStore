namespace MusicStore.Models
{
	public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Album album, int quantity)
        {
            CartLine? line = lineCollection
                .Where(p => p.Album.Id == album.Id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Album = album,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity++;
            }
        }

        public virtual void RemoveLine(Album album) => 
            lineCollection.RemoveAll(l => l.Album.Id == album.Id);

        public virtual decimal ComputeTotalValue() =>
            lineCollection.Sum(e => e.Album.Price * e.Quantity);

        public virtual void Clear() => 
            lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
}
