namespace MusicStore.Models
{
    public interface IAlbumRepository
    {
        IQueryable<Album> Albums { get; }
        Task AddAlbumAsync(Album album);
        Task UpdateAlbumAsync(Album album);
        Task DeleteAlbumAsync(int id);
    }
}
