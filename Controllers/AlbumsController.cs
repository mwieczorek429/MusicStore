using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;
using MusicStore.Services;

namespace MusicStore.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IFileService _fileService;

        public AlbumsController(IAlbumRepository context, IFileService fileService)
        {
            _albumRepository = context;
            _fileService = fileService;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            return View(await _albumRepository.Albums.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumRepository.Albums
                .FirstOrDefaultAsync(a => a.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Price,Genre,ReleaseYear,Duration,Description,ImageUrl")] Album album, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if(image != null) 
                {
                    try
                    {
                        string imageUrl = await _fileService.UploadImageAsync(image);
                        album.ImageUrl = imageUrl;
                    }
                    catch (ArgumentException ex)
                    {
                        ModelState.AddModelError("ImageUrl", ex.Message);
                        return View(album);
                    }
                }
                await _albumRepository.AddAlbumAsync(album);

                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumRepository.Albums
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Price,Genre,ReleaseYear,Duration,Description,ImageUrl")] Album album, IFormFile? image)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        try
                        {
                            string imageUrl = await _fileService.UploadImageAsync(image);
                            album.ImageUrl = imageUrl;
                        }
                        catch (ArgumentException ex)
                        {
                            ModelState.AddModelError("ImageUrl", ex.Message);
                            return View(album);
                        }
                    }
                    await _albumRepository.UpdateAlbumAsync(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumRepository.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _albumRepository.DeleteAlbumAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _albumRepository.Albums.Any(a => a.Id == id);
        }
    }
}
