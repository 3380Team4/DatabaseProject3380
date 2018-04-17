using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace ThemeParkApplication.Controllers
{
    public class MerchandiseController : Controller
    {
        private readonly themeparkdbContext _context;

        public MerchandiseController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Merchandise
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Merchandise.Include(m => m.Attr).Include(m => m.Conc).Include(m => m.ItemTypeNavigation);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Merchandise/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandise
                .Include(m => m.Attr)
                .Include(m => m.Conc)
                .Include(m => m.ItemTypeNavigation)
                .SingleOrDefaultAsync(m => m.ItemId == id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return View(merchandise);
        }

        // GET: Merchandise/Create
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId");
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId");
            ViewData["ItemType"] = new SelectList(_context.ItemTypeTable, "ItemTypeIndex", "ItemType");
            return View();
        }

        // POST: Merchandise/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create([Bind("ItemName,ItemId,Price,ItemType,ConcId,AttrId")] Merchandise merchandise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(merchandise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", merchandise.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", merchandise.ConcId);
            ViewData["ItemType"] = new SelectList(_context.ItemTypeTable, "ItemTypeIndex", "ItemType", merchandise.ItemType);
            return View(merchandise);
        }


        // GET: Merchandise/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandise.SingleOrDefaultAsync(m => m.ItemId == id);
            if (merchandise == null)
            {
                return NotFound();
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", merchandise.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", merchandise.ConcId);
            ViewData["ItemType"] = new SelectList(_context.ItemTypeTable, "ItemTypeIndex", "ItemType", merchandise.ItemType);
            return View(merchandise);
        }

        // POST: Merchandise/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("ItemName,ItemId,Price,ItemType,ConcId,AttrId")] Merchandise merchandise)
        {
            if (id != merchandise.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchandise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchandiseExists(merchandise.ItemId))
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", merchandise.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", merchandise.ConcId);
            ViewData["ItemType"] = new SelectList(_context.ItemTypeTable, "ItemTypeIndex", "ItemType", merchandise.ItemType);
            return View(merchandise);
        }

        // GET: Merchandise/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandise
                .Include(m => m.Attr)
                .Include(m => m.Conc)
                .Include(m => m.ItemTypeNavigation)
                .SingleOrDefaultAsync(m => m.ItemId == id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return View(merchandise);
        }

        // POST: Merchandise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var merchandise = await _context.Merchandise.SingleOrDefaultAsync(m => m.ItemId == id);
            _context.Merchandise.Remove(merchandise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MerchandiseExists(string id)
        {
            return _context.Merchandise.Any(e => e.ItemId == id);
        }
    }
}
