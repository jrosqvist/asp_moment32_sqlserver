using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moment32.Data;
using Moment32.Models;

namespace Moment32.Controllers
{
    public class CdController : Controller
    {
        private readonly CdContext _context;

        public CdController(CdContext context)
        {
            _context = context;
        }

        // GET: Cd
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            // Söksträng
            ViewData["CurrentFilter"] = searchString;

            // Sortering
            ViewData["ArtistSort"] = String.IsNullOrEmpty(sortOrder) ? "artist_desc" : "";
            ViewData["TitleSort"] = sortOrder == "Title" ? "title_desc" : "Title";
            

            var cdContext = from s in _context.Cd.Include(c => c.Artist)
                            select s;

            // Hämtar olika rader beroende på input
            if (!String.IsNullOrEmpty(searchString))
            {
                cdContext = cdContext.Where(s => s.Artist.Name.Contains(searchString)
                                       || s.Title.Contains(searchString)
                                       || s.Artist.Name.ToLower().Contains(searchString)
                                       || s.Title.ToLower().Contains(searchString)
                                       || s.Genre.ToLower().Contains(searchString)
                                       || s.Genre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "artist_desc":
                    cdContext  = cdContext.OrderByDescending(s => s.Artist);
                    break;
                case "Title":
                    cdContext = cdContext.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    cdContext = cdContext.OrderByDescending(s => s.Title);
                    break;
                default:
                    cdContext = cdContext.OrderBy(s => s.Artist);
                    break;
            }
            return View(await cdContext.AsNoTracking().ToListAsync());

        }

        // GET: Cd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd
                .Include(c => c.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // GET: Cd/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name");
            return View();
        }

        // POST: Cd/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistId,Title,Genre,ReleaseYear,NoOfSongs,PlayTime,NoOfCds")] Cd cd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: Cd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd.FindAsync(id);
            if (cd == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", cd.ArtistId);
            return View(cd);
        }

        // POST: Cd/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistId,Title,Genre,ReleaseYear,NoOfSongs,PlayTime,NoOfCds")] Cd cd)
        {
            if (id != cd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CdExists(cd.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: Cd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd
                .Include(c => c.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // POST: Cd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cd = await _context.Cd.FindAsync(id);
            _context.Cd.Remove(cd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CdExists(int id)
        {
            return _context.Cd.Any(e => e.Id == id);
        }
    }
}
