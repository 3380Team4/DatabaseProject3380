using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly themeparkdbContext _context;

        public MaintenancesController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Maintenances
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Maintenance.Include(m => m.Attr).Include(m => m.Conc).Include(m => m.MaintenanceEmployee).Include(m => m.OrderTypeNavigation).Include(m => m.WorkStatusNavigation);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenance
                .Include(m => m.Attr)
                .Include(m => m.Conc)
                .Include(m => m.MaintenanceEmployee)
                .Include(m => m.OrderTypeNavigation)
                .Include(m => m.WorkStatusNavigation)
                .SingleOrDefaultAsync(m => m.WorkOrderId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenances/Create
        public IActionResult Create()
        {
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId");
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId");
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType");
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatus");
            return View();
        }

        // POST: Maintenances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkOrderId,OrderType,ConcId,AttrId,MaintenanceEmployeeId,WorkStatus,WorkStartDate,WorkFinishDate")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", maintenance.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", maintenance.ConcId);
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", maintenance.MaintenanceEmployeeId);
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType", maintenance.OrderType);
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatus", maintenance.WorkStatus);
            return View(maintenance);
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenance.SingleOrDefaultAsync(m => m.WorkOrderId == id);
            if (maintenance == null)
            {
                return NotFound();
            }
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", maintenance.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", maintenance.ConcId);
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", maintenance.MaintenanceEmployeeId);
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType", maintenance.OrderType);
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatusIndex", maintenance.WorkStatus);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("WorkOrderId,OrderType,ConcId,AttrId,MaintenanceEmployeeId,WorkStatus,WorkStartDate,WorkFinishDate")] Maintenance maintenance)
        {
            if (id != maintenance.WorkOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceExists(maintenance.WorkOrderId))
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", maintenance.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", maintenance.ConcId);
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", maintenance.MaintenanceEmployeeId);
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType", maintenance.OrderType);
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatusIndex", maintenance.WorkStatus);
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenance
                .Include(m => m.Attr)
                .Include(m => m.Conc)
                .Include(m => m.MaintenanceEmployee)
                .Include(m => m.OrderTypeNavigation)
                .Include(m => m.WorkStatusNavigation)
                .SingleOrDefaultAsync(m => m.WorkOrderId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var maintenance = await _context.Maintenance.SingleOrDefaultAsync(m => m.WorkOrderId == id);
            _context.Maintenance.Remove(maintenance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(string id)
        {
            return _context.Maintenance.Any(e => e.WorkOrderId == id);
        }
    }
}
