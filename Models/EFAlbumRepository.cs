using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Services;

namespace MusicStore.Models
{
    public class EFAlbumRepository : IAlbumRepository
    {
        MusicStoreDbContext _context;
        public EFAlbumRepository(MusicStoreDbContext context) 
        {
            _context = context;
        }
        public IQueryable<Album> Albums => _context.Album;

        public async Task AddAlbumAsync(Album album)
        {
            _context.Album.Add(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _context.Album
                .FirstOrDefaultAsync(a => a.Id == id);
            if (album != null) 
            {
                _context.Album.Remove(album);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateAlbumAsync(Album album)
        {
            _context.Update(album);
            await _context.SaveChangesAsync();
        }
    }
}
