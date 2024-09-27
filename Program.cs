using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicStore.Areas.Identity.Data;
using MusicStore.Configurations;
using MusicStore.Data;
using MusicStore.Extensions;
using MusicStore.Models;
using MusicStore.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MusicStoreDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MusicStoreDbContextConnection' not found.");

builder.Services.AddDbContext<MusicStoreDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddIdentity<MusicStoreUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<MusicStoreDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAlbumRepository, EFAlbumRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddRazorPages();
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));
builder.Services.AddTransient<PayPalConfiguration>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Albums}/{action=Index}/{id?}");

app.MapRazorPages();
await app.Services.InitializeRolesAsync();

app.Run();
