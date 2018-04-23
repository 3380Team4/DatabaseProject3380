using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly themeparkdbContext _context;
        public HomeController(themeparkdbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            var dataWeekly = new List<DataPoint>();
            int[] weeklyCustomer = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            var themeparkdbContext = _context.Transactions;

            foreach (Transactions t in themeparkdbContext)
            {
                for (var Day = DateTime.Now.Day - 7; Day < DateTime.Now.Day; Day++)
                {
                    if (t.DateOfSale.Day == Day)
                    {
                        if (t.DateOfSale.DayOfWeek == DayOfWeek.Monday)
                            weeklyCustomer[0]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Tuesday)
                            weeklyCustomer[1]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Wednesday)
                            weeklyCustomer[2]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Thursday)
                            weeklyCustomer[3]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Friday)
                            weeklyCustomer[4]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Saturday)
                            weeklyCustomer[5]++;
                        else if (t.DateOfSale.DayOfWeek == DayOfWeek.Sunday)
                            weeklyCustomer[6]++;
                    }
                }

            }

            dataWeekly.Add(new DataPoint("Mon", weeklyCustomer[0]));
            dataWeekly.Add(new DataPoint("Tue", weeklyCustomer[1]));
            dataWeekly.Add(new DataPoint("Wed", weeklyCustomer[2]));
            dataWeekly.Add(new DataPoint("thu", weeklyCustomer[3]));
            dataWeekly.Add(new DataPoint("Fri", weeklyCustomer[4]));
            dataWeekly.Add(new DataPoint("Sat", weeklyCustomer[5]));
            dataWeekly.Add(new DataPoint("Sun", weeklyCustomer[6]));

           
            ViewBag.dataWeekly = JsonConvert.SerializeObject(dataWeekly);

            return View();
        }
        
        public IActionResult MonthlyCustomer()
        {
            var dataMonthly = new List<DataPoint>();
            int[] monthlyCustomer = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var themeparkdbContext = _context.Transactions;

            foreach (Transactions t in themeparkdbContext)
            {
                if (t.DateOfSale.Year == 2018)
                {
                    if (t.DateOfSale.Month == 1)
                        monthlyCustomer[0]++;
                    else if (t.DateOfSale.Month == 2)
                        monthlyCustomer[1]++;
                    else if (t.DateOfSale.Month == 3)
                        monthlyCustomer[2]++;
                    else if (t.DateOfSale.Month == 4)
                    {
                        monthlyCustomer[3]++;
                        
                    }
                }
                else if (t.DateOfSale.Year == 2017)
                {
                    if (t.DateOfSale.Month == 12)
                        monthlyCustomer[11]++;
                    else if (t.DateOfSale.Month == 11)
                        monthlyCustomer[10]++;
                    else if (t.DateOfSale.Month == 10)
                        monthlyCustomer[9]++;
                    else if (t.DateOfSale.Month == 9)
                        monthlyCustomer[8]++;
                    else if (t.DateOfSale.Month == 8)
                        monthlyCustomer[7]++;
                    else if (t.DateOfSale.Month == 7)
                        monthlyCustomer[6]++;
                    else if (t.DateOfSale.Month == 6)
                        monthlyCustomer[5]++;
                    else if (t.DateOfSale.Month == 5)
                        monthlyCustomer[4]++;
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

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
