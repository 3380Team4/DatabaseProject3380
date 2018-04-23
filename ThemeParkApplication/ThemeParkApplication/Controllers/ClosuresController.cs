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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId");
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId");
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", closures.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", closures.ConcId);
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
            ViewData["AttrId"] = new SelectList(_context.Attractions, "AttractionId", "AttractionId", closures.AttrId);
            ViewData["ConcId"] = new SelectList(_context.Concessions, "ConcessionId", "ConcessionId", closures.ConcId);
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

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult NumberOfClosuresStatistic()
        {
            var dataMonthly = new List<DataPoint>();
            double[] monthlyCustomer = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var themeparkdbContext = _context.Closures;

            foreach (Closures t in themeparkdbContext)
            {
                if (t.Reason == 1)
                {
                    if (t.DateClosure.Month == 1)
                        monthlyCustomer[0]++;
                    else if (t.DateClosure.Month == 2)
                        monthlyCustomer[1]++;
                    else if (t.DateClosure.Month == 3)
                        monthlyCustomer[2]++;
                    else if (t.DateClosure.Month == 4)
                        monthlyCustomer[3]++;
                    else if (t.DateClosure.Month == 5)
                        monthlyCustomer[4]++;
                    else if (t.DateClosure.Month == 6)
                        monthlyCustomer[5]++;
                    else if (t.DateClosure.Month == 7)
                        monthlyCustomer[6]++;
                    else if (t.DateClosure.Month == 8)
                        monthlyCustomer[7]++;
                    else if (t.DateClosure.Month == 9)
                        monthlyCustomer[8]++;
                    else if (t.DateClosure.Month == 10)
                        monthlyCustomer[9]++;
                    else if (t.DateClosure.Month == 11)
                        monthlyCustomer[10]++;
                    else if (t.DateClosure.Month == 12)
                        monthlyCustomer[11]++;
                }
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
