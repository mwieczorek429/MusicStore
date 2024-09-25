using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicStore.Areas.Identity.Data;
using MusicStore.Data;
using MusicStore.Extensions;
using MusicStore.Models;
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
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddRazorPages();

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
