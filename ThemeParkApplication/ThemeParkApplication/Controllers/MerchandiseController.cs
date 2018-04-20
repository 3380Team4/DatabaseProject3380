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
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["SortByItemName"] = sortOrder == "sort_item_name_desc"?"sort_item_name_asc" : "sort_item_name_desc";
            ViewData["SortByPrice"] = sortOrder == "sort_price_desc" ? "sort_price_asc" : "sort_price_desc";
            ViewData["SortByAttraction"] = sortOrder == "sort_attraction_desc" ? "sort_attraction_asc" : "sort_attraction_desc";
            ViewData["SortByConcession"] = sortOrder == "sort_concession_desc" ? "sort_concession_asc" : "sort_concession_desc";
            ViewData["SortByItemType"] = sortOrder == "sort_item_type_desc" ? "sort_item_type_asc" : "sort_item_type_desc";
            var themeparkdbContext = from s in _context.Merchandise.Include(m => m.Attr).Include(m => m.Conc).Include(m => m.ItemTypeNavigation)
                                     select s;      
            switch (sortOrder)
            {
                case "sort_item_name_asc":
                    themeparkdbContext = themeparkdbContext.OrderBy(s => s.ItemName);
                    break;

                case "sort_item_name_desc":
                    themeparkdbContext = themeparkdbContext.OrderByDescending(s => s.ItemName);
                    break;
                case "sort_price_asc":
                    themeparkdbContext = themeparkdbContext.OrderBy(s => s.Price);
                    break;

                case "sort_price_desc":
                    themeparkdbContext = themeparkdbContext.OrderByDescending(s => s.Price);
                    break;
                case "sort_attraction_asc":
                    themeparkdbContext = themeparkdbContext.OrderBy(s => s.Attr.AttractionName);
                    break;
                case "sort_attraction_desc":
                    themeparkdbContext = themeparkdbContext.OrderByDescending(s => s.Attr.AttractionName);
                    break;
                case "sort_concession_asc":
                    themeparkdbContext = themeparkdbContext.OrderBy(s => s.Conc.ConcessionName);
                    break;
                case "sort_concession_desc":
                    themeparkdbContext = themeparkdbContext.OrderByDescending(s => s.Conc.StoreType);
                    break;
                case "sort_item_type_asc":
                    themeparkdbContext = themeparkdbContext.OrderBy(s => s.ItemType);
                    break;
                case "sort_item_type_desc":
                    themeparkdbContext = themeparkdbContext.OrderByDescending(s => s.ItemType);
                    break;
                default:
                    break;
            }


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
