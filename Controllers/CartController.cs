using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MusicStore.Models;
using NuGet.Protocol;
using MusicStore.Extensions;
using MusicStore.Data;
using MusicStore.Models.ViewModels;

namespace MusicStore.Controllers
{
    public class CartController : Controller
    {

        private readonly MusicStoreDbContext _context;
        private Cart _cart; 

        public CartController(MusicStoreDbContext context, Cart cartService) 
        {
           _context = context;
           _cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }

        public IActionResult AddToCart(int albumId, string returnUrl) 
        {
            Album? album = _context.Album.Where(a => a.Id == albumId).FirstOrDefault();

            if (album == null)
            {
                return NotFound();
            }

            _cart.AddItem(album, 1);

            return RedirectToAction("Index", new {returnUrl });
        }

        public IActionResult RemoveFromCart(int albumId, string returnUrl) 
        {
            Album? album = _context.Album.Where(a => a.Id == albumId).FirstOrDefault();

            if (album == null)
            {
                return NotFound();
            }

            _cart.RemoveLine(album);

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
