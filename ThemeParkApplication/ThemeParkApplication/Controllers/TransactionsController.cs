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
    public class TransactionsController : Controller
    {
        private readonly themeparkdbContext _context;

        public TransactionsController(themeparkdbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var themeparkdbContext = _context.Transactions.Include(t => t.Guest).Include(t => t.Merch).Include(t => t.SellerEmployee);
            return View(await themeparkdbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Guest)
                .Include(t => t.Merch)
                .Include(t => t.SellerEmployee)
                .SingleOrDefaultAsync(m => m.TransactionId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // GET: Transactions/Create
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            ViewData["GuestId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFirstName");
            ViewData["MerchId"] = new SelectList(_context.Merchandise, "ItemId", "ItemId");
            ViewData["SellerEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,DateOfSale,MerchId,SaleAmount,SellerEmployeeId,GuestId,Quantity")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GuestId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFirstName", transactions.GuestId);
            ViewData["MerchId"] = new SelectList(_context.Merchandise, "ItemId", "ItemId", transactions.MerchId);
            ViewData["SellerEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", transactions.SellerEmployeeId);
            return View(transactions);
        }

        // GET: Transactions/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions.SingleOrDefaultAsync(m => m.TransactionId == id);
            if (transactions == null)
            {
                return NotFound();
            }
            ViewData["GuestId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFirstName", transactions.GuestId);
            ViewData["MerchId"] = new SelectList(_context.Merchandise, "ItemId", "ItemId", transactions.MerchId);
            ViewData["SellerEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", transactions.SellerEmployeeId);
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("TransactionId,DateOfSale,MerchId,SaleAmount,SellerEmployeeId,GuestId,Quantity")] Transactions transactions)
        {
            if (id != transactions.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionsExists(transactions.TransactionId))
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
            ViewData["GuestId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFirstName", transactions.GuestId);
            ViewData["MerchId"] = new SelectList(_context.Merchandise, "ItemId", "ItemId", transactions.MerchId);
            ViewData["SellerEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", transactions.SellerEmployeeId);
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Guest)
                .Include(t => t.Merch)
                .Include(t => t.SellerEmployee)
                .SingleOrDefaultAsync(m => m.TransactionId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transactions = await _context.Transactions.SingleOrDefaultAsync(m => m.TransactionId == id);
            _context.Transactions.Remove(transactions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionsExists(string id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
