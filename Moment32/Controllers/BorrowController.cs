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
    public class BorrowController : Controller
    {
        private readonly CdContext _context;

        public BorrowController(CdContext context)
        {
            _context = context;
        }

        // GET: Borrow
        public async Task<IActionResult> Index()
        {
            var cdContext = _context.Borrow.Include(b => b.Cd);
            return View(await cdContext.ToListAsync());
        }

        // GET: Borrow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow
                .Include(b => b.Cd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrow/Create
        public IActionResult Create()
        {

            // Hämtar alla skivor som inte redan är utlånade
            var cdContext = _context.Cd.Where(s => s.Borrowed == false)
                .Select(s => s).ToList();

            foreach (var c in cdContext)
            {
                c.Title += " - " + c.ReleaseYear;
            }

            // Lägger till dem i formuläret
            ViewData["CdId"] = new SelectList(cdContext, "Id", "Title");
            //ViewData["CdId"] = new SelectList(_context.Cd, "Id", "Title");

            return View();
        }

        // POST: Borrow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User,BorrowDate,CdId")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                // Hämtar posten med samma ID som angivet
                var query =
                   from c in _context.Cd
                   where c.Id == borrow.CdId
                   select c;

                // Ändrar status till utlånad
                foreach (var c in query)
                {
                    c.Borrowed = true;
                }

                _context.Add(borrow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CdId"] = new SelectList(_context.Cd, "Id", "Title", borrow.CdId);
            return View(borrow);
        }

        // GET: Borrow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }

            // Hämtar alla skivor som inte redan är utlånade
            var cdContext = _context.Cd.Where(s => s.Borrowed == false
                || s.Id == borrow.CdId
                ).Select(s => s);

            var cdList = cdContext.ToList();

            // Lägger till dem i formuläret
            ViewData["CdId"] = new SelectList(cdList, "Id", "Title", borrow.CdId);
            //ViewData["CdId"] = new SelectList(_context.Cd, "Id", "Title", borrow.CdId);
            return View(borrow);
        }

        // POST: Borrow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User,BorrowDate,CdId")] Borrow borrow)
        {
            if (id != borrow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.Id))
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
            ViewData["CdId"] = new SelectList(_context.Cd, "Id", "Title", borrow.CdId);
            return View(borrow);
        }

        // GET: Borrow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow
                .Include(b => b.Cd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // POST: Borrow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrow = await _context.Borrow.FindAsync(id);
            _context.Borrow.Remove(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrow.Any(e => e.Id == id);
        }
    }
}
