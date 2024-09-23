using Microsoft.EntityFrameworkCore;
using MusicStore.Areas.Identity.Data;
using MusicStore.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MusicStoreDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MusicStoreDbContextConnection' not found.");

builder.Services.AddDbContext<MusicStoreDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<MusicStoreUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MusicStoreDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapRazorPages();

app.Run();
