using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStore.Areas.Identity.Data;
using MusicStore.Models;

namespace MusicStore.Data;

public class MusicStoreDbContext : IdentityDbContext<MusicStoreUser>
{
    public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<MusicStore.Models.Album> Album { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
}
