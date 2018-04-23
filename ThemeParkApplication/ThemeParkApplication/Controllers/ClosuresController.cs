﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ThemeParkApplication.Controllers
{
    public class ClosuresController : Controller
    {
        private readonly themeparkdbContext _context;

        public ClosuresController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Closures
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Closures.Include(c => c.Attr).Include(c => c.Conc).Include(c => c.ReasonNavigation);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Closures/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var closures = await _context.Closures
                .Include(c => c.Attr)
                .Include(c => c.Conc)
                .Include(c => c.ReasonNavigation)
                .SingleOrDefaultAsync(m => m.ClosureId == id);
            if (closures == null)
            {
                return NotFound();
            }

            return View(closures);
        }

        // GET: Closures/Create
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName");
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName");
            ViewData["Reason"] = new SelectList(_context.ReasonTable, "ReasonIndex", "Reason");
            return View();
        }

        // POST: Closures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create([Bind("ClosureId,Reason,ConcId,AttrId,DateClosure,DurationClosure")] Closures closures)
        {
            if (ModelState.IsValid)
            {
                _context.Add(closures);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName", closures.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName", closures.ConcId);
            ViewData["Reason"] = new SelectList(_context.ReasonTable, "ReasonIndex", "Reason", closures.Reason);
            return View(closures);
        }

        // GET: Closures/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var closures = await _context.Closures.SingleOrDefaultAsync(m => m.ClosureId == id);
            if (closures == null)
            {
                return NotFound();
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName", closures.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName", closures.ConcId);
            ViewData["Reason"] = new SelectList(_context.ReasonTable, "ReasonIndex", "Reason", closures.Reason);
            return View(closures);
        }

        // POST: Closures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("ClosureId,Reason,ConcId,AttrId,DateClosure,DurationClosure")] Closures closures)
        {
            if (id != closures.ClosureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(closures);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClosuresExists(closures.ClosureId))
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", closures.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", closures.ConcId);
            ViewData["Reason"] = new SelectList(_context.ReasonTable, "ReasonIndex", "Reason", closures.Reason);
            return View(closures);
        }

        // GET: Closures/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var closures = await _context.Closures
                .Include(c => c.Attr)
                .Include(c => c.Conc)
                .Include(c => c.ReasonNavigation)
                .SingleOrDefaultAsync(m => m.ClosureId == id);
            if (closures == null)
            {
                return NotFound();
            }

            return View(closures);
        }

        // POST: Closures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var closures = await _context.Closures.SingleOrDefaultAsync(m => m.ClosureId == id);
            _context.Closures.Remove(closures);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClosuresExists(string id)
        {
            return _context.Closures.Any(e => e.ClosureId == id);
        }
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> generateReportAsync(string YearNumber, string MonthNumber, string ToYearNumber, string ToMonthNumber)
        {
            TempData["year"] = YearNumber;
            TempData["month"] = MonthNumber;
            TempData["toYear"] = ToYearNumber;
            TempData["toMonth"] = ToMonthNumber;
            var query = String.Format("SELECT * FROM dbo.Closures Where (Reason = 2 and Date_Closure >= '{0}/01/{1}' and Date_Closure <'{2}/01/{3}')", MonthNumber, YearNumber, ToMonthNumber, ToYearNumber);
            var report = _context.Closures.FromSql(query);
            return View(await report.ToListAsync());
        }
    }
}
