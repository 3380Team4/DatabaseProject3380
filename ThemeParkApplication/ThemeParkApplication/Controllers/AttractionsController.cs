using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Controllers
{   
    public class AttractionsController : Controller
    {
        private readonly themeparkdbContext _context;

        public AttractionsController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Attractions
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Attractions.Include(a => a.AttractionStatusNavigation).Include(a => a.AttractionTypeNavigation).Include(a => a.Manager);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Attractions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractions = await _context.Attractions
                .Include(a => a.AttractionStatusNavigation)
                .Include(a => a.AttractionTypeNavigation)
                .Include(a => a.Manager)
                .SingleOrDefaultAsync(m => m.AttractionId == id);
            if (attractions == null)
            {
                return NotFound();
            }

            return View(attractions);
        }

        // GET: Attractions/Create
        public IActionResult Create()
        {
            ViewData["AttractionStatus"] = new SelectList(_context.AttractionStatusTable, "AttractionStatusIndex", "AttractionStatus");
            ViewData["AttractionType"] = new SelectList(_context.AttractionTypeTable, "AttractionTypeIndex", "AttractionType");
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: Attractions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttractionName,AttractionId,ManagerId,AttractionType,HeightRequirement,AgeRequirement,AttractionStatus")] Attractions attractions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attractions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttractionStatus"] = new SelectList(_context.AttractionStatusTable, "AttractionStatusIndex", "AttractionStatus", attractions.AttractionStatus);
            ViewData["AttractionType"] = new SelectList(_context.AttractionTypeTable, "AttractionTypeIndex", "AttractionType", attractions.AttractionType);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", attractions.ManagerId);
            return View(attractions);
        }

        // GET: Attractions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractions = await _context.Attractions.SingleOrDefaultAsync(m => m.AttractionId == id);
            if (attractions == null)
            {
                return NotFound();
            }
            ViewData["AttractionStatus"] = new SelectList(_context.AttractionStatusTable, "AttractionStatusIndex", "AttractionStatus", attractions.AttractionStatus);
            ViewData["AttractionType"] = new SelectList(_context.AttractionTypeTable, "AttractionTypeIndex", "AttractionType", attractions.AttractionType);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", attractions.ManagerId);
            return View(attractions);
        }

        // POST: Attractions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AttractionName,AttractionId,ManagerId,AttractionType,HeightRequirement,AgeRequirement,AttractionStatus")] Attractions attractions)
        {
            if (id != attractions.AttractionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attractions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionsExists(attractions.AttractionId))
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
            ViewData["AttractionStatus"] = new SelectList(_context.AttractionStatusTable, "AttractionStatusIndex", "AttractionStatus", attractions.AttractionStatus);
            ViewData["AttractionType"] = new SelectList(_context.AttractionTypeTable, "AttractionTypeIndex", "AttractionType", attractions.AttractionType);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", attractions.ManagerId);
            return View(attractions);
        }

        // GET: Attractions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractions = await _context.Attractions
                .Include(a => a.AttractionStatusNavigation)
                .Include(a => a.AttractionTypeNavigation)
                .Include(a => a.Manager)
                .SingleOrDefaultAsync(m => m.AttractionId == id);
            if (attractions == null)
            {
                return NotFound();
            }

            return View(attractions);
        }

        // POST: Attractions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var attractions = await _context.Attractions.SingleOrDefaultAsync(m => m.AttractionId == id);
            _context.Attractions.Remove(attractions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractionsExists(string id)
        {
            return _context.Attractions.Any(e => e.AttractionId == id);
        }
    }
}
