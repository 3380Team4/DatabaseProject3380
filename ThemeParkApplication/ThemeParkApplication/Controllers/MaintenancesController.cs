using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThemeParkApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["ShowRequestedParm"] = String.IsNullOrEmpty(sortOrder) ? "show_requested" : "show_requested";
            ViewData["ShowInProgressParm"] = String.IsNullOrEmpty(sortOrder) ? "show_in_progress" : "show_in_progress";
            ViewData["ShowCompletedParm"] = String.IsNullOrEmpty(sortOrder) ? "show_completed" : "show_completed";
            ViewData["ShowAllParm"] = String.IsNullOrEmpty(sortOrder) ? "show_all" : "show_all";
            
            var themeparkdbContext = from s in _context.Maintenance.Include(m => m.Attr).Include(m => m.Conc).Include(m => m.MaintenanceEmployee).Include(m => m.OrderTypeNavigation).Include(m => m.WorkStatusNavigation) select s;

            switch (sortOrder)
            {
                case "show_requested":
                    //  concessions = concessions.OrderByDescending(s => s.ConcessionName);
                    themeparkdbContext = themeparkdbContext.Where(s => s.WorkStatus.Equals(0));
                    break;
                case "show_in_progress":
                    themeparkdbContext = themeparkdbContext.Where(s => s.WorkStatus.Equals(1));
                    break;
                case "show_completed":
                    themeparkdbContext = themeparkdbContext.Where(s => s.WorkStatus.Equals(2));
                    break;
                case "show_all":
                    themeparkdbContext = themeparkdbContext.Where(s => s.WorkStatus.Equals(1) || s.WorkStatus.Equals(0) || s.WorkStatus.Equals(2));
                    break;

                default:
                    break;
            }
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
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName");
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName");
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName", maintenance.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName", maintenance.ConcId);
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", maintenance.MaintenanceEmployeeId);
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType", maintenance.OrderType);
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatus", maintenance.WorkStatus);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionName", maintenance.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionName", maintenance.ConcId);
            ViewData["MaintenanceEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", maintenance.MaintenanceEmployeeId);
            ViewData["OrderType"] = new SelectList(_context.OrderTypeTable, "OrderTypeIndex", "OrderType", maintenance.OrderType);
            ViewData["WorkStatus"] = new SelectList(_context.WorkStatusTable, "WorkStatusIndex", "WorkStatus", maintenance.WorkStatus);
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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

        public async Task<IActionResult> generateReportAsync(string YearNumber, string MonthNumber)
        {
            var month = Int32.Parse(MonthNumber);
            var Year = Int32.Parse(YearNumber);

            if (month == 12)
            {
                month = 1;
                Year++;

            }
            else
            {
                month++;
            }
            var toMonth = month.ToString();
            var toYear = Year.ToString();

            var query = String.Format("SELECT * FROM dbo.Maintenance Where (Work_Start_Date >= '{0}/01/{1}' and Work_Start_Date < '{2}/01/{3}')", MonthNumber, YearNumber, toMonth, toYear);
            var report = _context.Maintenance.FromSql(query);
            return View(await report.ToListAsync());
        }
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> NumberOfMaintenances(string YearNumber, string MonthNumber, string ToYearNumber, string ToMonthNumber)
        {
            TempData["year"] = YearNumber;
            TempData["month"] = MonthNumber;
            TempData["toYear"] = ToYearNumber;
            TempData["toMonth"] = ToMonthNumber;
            var query = String.Format("SELECT * FROM dbo.Maintenance Where ( Work_Start_Date >= '{0}/01/{1}' and Work_Start_Date <'{2}/01/{3}')", MonthNumber, YearNumber, ToMonthNumber, ToYearNumber);
            var report = _context.Maintenance.FromSql(query);
            return View(await report.ToListAsync());
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult NumberOfMaintenancesStatistic()
        {
            var dataMonthly = new List<DataPoint>();
            double[] monthlyCustomer = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var themeparkdbContext = _context.Maintenance;

            foreach (Maintenance t in themeparkdbContext)
            {
                if (t.WorkStartDate.Month == 1)
                    monthlyCustomer[0]++;
                else if (t.WorkStartDate.Month == 2)
                    monthlyCustomer[1]++;
                else if (t.WorkStartDate.Month == 3)
                    monthlyCustomer[2]++;
                else if (t.WorkStartDate.Month == 4)
                    monthlyCustomer[3]++;
                else if (t.WorkStartDate.Month == 5)
                    monthlyCustomer[4]++;
                else if (t.WorkStartDate.Month == 6)
                    monthlyCustomer[5]++;
                else if (t.WorkStartDate.Month == 7)
                    monthlyCustomer[6]++;
                else if (t.WorkStartDate.Month == 8)
                    monthlyCustomer[7]++;
                else if (t.WorkStartDate.Month == 9)
                    monthlyCustomer[8]++;
                else if (t.WorkStartDate.Month == 10)
                    monthlyCustomer[9]++;
                else if (t.WorkStartDate.Month == 11)
                    monthlyCustomer[10]++;
                else if (t.WorkStartDate.Month == 12)
                    monthlyCustomer[11]++;
            }

            dataMonthly.Add(new DataPoint("Jan", monthlyCustomer[0]));
            dataMonthly.Add(new DataPoint("Feb", monthlyCustomer[1]));
            dataMonthly.Add(new DataPoint("Mar", monthlyCustomer[2]));
            dataMonthly.Add(new DataPoint("Apr", monthlyCustomer[3]));
            dataMonthly.Add(new DataPoint("May", monthlyCustomer[4]));
            dataMonthly.Add(new DataPoint("Jun", monthlyCustomer[5]));
            dataMonthly.Add(new DataPoint("Jul", monthlyCustomer[6]));
            dataMonthly.Add(new DataPoint("Aug", monthlyCustomer[7]));
            dataMonthly.Add(new DataPoint("Sep", monthlyCustomer[8]));
            dataMonthly.Add(new DataPoint("Oct", monthlyCustomer[9]));
            dataMonthly.Add(new DataPoint("Nov", monthlyCustomer[10]));
            dataMonthly.Add(new DataPoint("Dec", monthlyCustomer[11]));

            ViewBag.dataMonthly = JsonConvert.SerializeObject(dataMonthly);

            return View();
        }

    }
}