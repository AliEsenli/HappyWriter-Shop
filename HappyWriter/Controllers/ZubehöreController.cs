using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HappyWriter.Data;
using HappyWriter.Models;
using Microsoft.AspNetCore.Authorization;

namespace HappyWriter.Views
{
    [Authorize(Roles = "Admin")]
    public class ZubehöreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZubehöreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zubehör
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zubehöre.ToListAsync());
        }

        // GET: Zubehör/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zubehör = await _context.Zubehöre
                .FirstOrDefaultAsync(m => m.ZubehörId == id);
            if (zubehör == null)
            {
                return NotFound();
            }

            return View(zubehör);
        }

        // GET: Zubehör/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zubehör/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZubehörId,ZubehörName,ZubehörKosten")] Zubehör zubehör)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zubehör);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zubehör);
        }

        // GET: Zubehör/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zubehör = await _context.Zubehöre.FindAsync(id);
            if (zubehör == null)
            {
                return NotFound();
            }
            return View(zubehör);
        }

        // POST: Zubehör/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZubehörId,ZubehörName,ZubehörKosten")] Zubehör zubehör)
        {
            if (id != zubehör.ZubehörId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zubehör);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZubehörExists(zubehör.ZubehörId))
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
            return View(zubehör);
        }

        // GET: Zubehör/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zubehör = await _context.Zubehöre
                .FirstOrDefaultAsync(m => m.ZubehörId == id);
            if (zubehör == null)
            {
                return NotFound();
            }

            return View(zubehör);
        }

        // POST: Zubehör/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zubehör = await _context.Zubehöre.FindAsync(id);
            _context.Zubehöre.Remove(zubehör);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZubehörExists(int id)
        {
            return _context.Zubehöre.Any(e => e.ZubehörId == id);
        }
    }
}
