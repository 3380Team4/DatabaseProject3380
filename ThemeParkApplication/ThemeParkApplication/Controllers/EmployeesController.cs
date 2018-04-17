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
    public class EmployeesController : Controller
    {
        private readonly themeparkdbContext _context;

        public EmployeesController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Employees.Include(e => e.JobTitleNavigation).Include(e => e.Supervisor).Include(e => e.WorksAtAttrNavigation).Include(e => e.WorksAtConcNavigation);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.JobTitleNavigation)
                .Include(e => e.Supervisor)
                .Include(e => e.WorksAtAttrNavigation)
                .Include(e => e.WorksAtConcNavigation)
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            ViewData["JobTitle"] = new SelectList(_context.JobTitleTable, "JobTitleIndex", "JobTitle");
            ViewData["SupervisorId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["WorksAtAttr"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId");
            ViewData["WorksAtConc"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,EmployeeId,Gender,DateBirth,JobTitle,SupervisorId,StartDate,EndDate,WorksAtConc,WorksAtAttr")] Employees employees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobTitle"] = new SelectList(_context.JobTitleTable, "JobTitleIndex", "JobTitle", employees.JobTitle);
            ViewData["SupervisorId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", employees.SupervisorId);
            ViewData["WorksAtAttr"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", employees.WorksAtAttr);
            ViewData["WorksAtConc"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", employees.WorksAtConc);
            return View(employees);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }
            ViewData["JobTitle"] = new SelectList(_context.JobTitleTable, "JobTitleIndex", "JobTitle", employees.JobTitle);
            ViewData["SupervisorId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", employees.SupervisorId);
            ViewData["WorksAtAttr"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", employees.WorksAtAttr);
            ViewData["WorksAtConc"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", employees.WorksAtConc);
            return View(employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("LastName,FirstName,EmployeeId,Gender,DateBirth,JobTitle,SupervisorId,StartDate,EndDate,WorksAtConc,WorksAtAttr")] Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.EmployeeId))
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
            ViewData["JobTitle"] = new SelectList(_context.JobTitleTable, "JobTitleIndex", "JobTitle", employees.JobTitle);
            ViewData["SupervisorId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", employees.SupervisorId);
            ViewData["WorksAtAttr"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", employees.WorksAtAttr);
            ViewData["WorksAtConc"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", employees.WorksAtConc);
            return View(employees);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.JobTitleNavigation)
                .Include(e => e.Supervisor)
                .Include(e => e.WorksAtAttrNavigation)
                .Include(e => e.WorksAtConcNavigation)
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employees = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(string id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
