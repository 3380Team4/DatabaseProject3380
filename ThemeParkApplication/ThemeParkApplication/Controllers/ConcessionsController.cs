﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Controllers
{
    public class ConcessionsController : Controller
    {
        private readonly themeparkdbContext _context;

        public ConcessionsController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Concessions
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Concessions.Include(c => c.ConcessionStatusNavigation).Include(c => c.Manager).Include(c => c.StoreTypeNavigation);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Concessions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessions = await _context.Concessions
                .Include(c => c.ConcessionStatusNavigation)
                .Include(c => c.Manager)
                .Include(c => c.StoreTypeNavigation)
                .SingleOrDefaultAsync(m => m.ConcessionId == id);
            if (concessions == null)
            {
                return NotFound();
            }

            return View(concessions);
        }

        // GET: Concessions/Create
        public IActionResult Create()
        {
            ViewData["ConcessionStatus"] = new SelectList(_context.ConcessionStatusTable, "ConcessionStatusIndex", "ConcessionStatus");
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["StoreType"] = new SelectList(_context.StoreTypeTable, "StoreTypeIndex", "StoreType");
            return View();
        }

        // POST: Concessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcessionName,ConcessionId,ManagerId,StoreType,OpeningTime,ClosingTime,ConcessionStatus")] Concessions concessions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concessions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcessionStatus"] = new SelectList(_context.ConcessionStatusTable, "ConcessionStatusIndex", "ConcessionStatus", concessions.ConcessionStatus);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", concessions.ManagerId);
            ViewData["StoreType"] = new SelectList(_context.StoreTypeTable, "StoreTypeIndex", "StoreType", concessions.StoreType);
            return View(concessions);
        }

        // GET: Concessions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessions = await _context.Concessions.SingleOrDefaultAsync(m => m.ConcessionId == id);
            if (concessions == null)
            {
                return NotFound();
            }
            ViewData["ConcessionStatus"] = new SelectList(_context.ConcessionStatusTable, "ConcessionStatusIndex", "ConcessionStatus", concessions.ConcessionStatus);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", concessions.ManagerId);
            ViewData["StoreType"] = new SelectList(_context.StoreTypeTable, "StoreTypeIndex", "StoreType", concessions.StoreType);
            return View(concessions);
        }

        // POST: Concessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcessionName,ConcessionId,ManagerId,StoreType,OpeningTime,ClosingTime,ConcessionStatus")] Concessions concessions)
        {
            if (id != concessions.ConcessionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concessions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcessionsExists(concessions.ConcessionId))
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
            ViewData["ConcessionStatus"] = new SelectList(_context.ConcessionStatusTable, "ConcessionStatusIndex", "ConcessionStatus", concessions.ConcessionStatus);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", concessions.ManagerId);
            ViewData["StoreType"] = new SelectList(_context.StoreTypeTable, "StoreTypeIndex", "StoreType", concessions.StoreType);
            return View(concessions);
        }

        // GET: Concessions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessions = await _context.Concessions
                .Include(c => c.ConcessionStatusNavigation)
                .Include(c => c.Manager)
                .Include(c => c.StoreTypeNavigation)
                .SingleOrDefaultAsync(m => m.ConcessionId == id);
            if (concessions == null)
            {
                return NotFound();
            }

            return View(concessions);
        }

        // POST: Concessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concessions = await _context.Concessions.SingleOrDefaultAsync(m => m.ConcessionId == id);
            _context.Concessions.Remove(concessions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcessionsExists(string id)
        {
            return _context.Concessions.Any(e => e.ConcessionId == id);
        }
    }
}
