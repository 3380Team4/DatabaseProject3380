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
    public class ConcessionsController : Controller
    {
        private readonly themeparkdbContext _context;

        public ConcessionsController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Concessions
        public async Task<IActionResult> Index(string sortOrder)
        {
          //  var themeparkdbContext = _context.Concessions.Include(c => c.ConcessionStatusNavigation).Include(c => c.Manager).Include(c => c.StoreTypeNavigation);

            ViewData["ShowOpenParm"] = String.IsNullOrEmpty(sortOrder) ? "show_open" : "show_open";
            ViewData["ShowClosedParm"] = String.IsNullOrEmpty(sortOrder) ? "show_closed" : "show_closed";
            ViewData["ShowAllParm"] = String.IsNullOrEmpty(sortOrder) ? "show_all" : "show_all";
            var concessions = from s in _context.Concessions.Include(c => c.ConcessionStatusNavigation).Include(c => c.Manager).Include(c => c.StoreTypeNavigation)
                              select s;
            switch (sortOrder)
            {
                case "show_open":
                  //  concessions = concessions.OrderByDescending(s => s.ConcessionName);
                    concessions = concessions.Where(s => s.ConcessionStatus.Equals(0));
                    break;
                case "show_closed":
                    concessions = concessions.Where(s => s.ConcessionStatus.Equals(1));
                    break;
                case "show_all":
                    concessions = concessions.Where(s => s.ConcessionStatus.Equals(1) || s.ConcessionStatus.Equals(0));
                    break;

                default:
                    break;
            }
            return View(await concessions.AsNoTracking().ToListAsync());
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
