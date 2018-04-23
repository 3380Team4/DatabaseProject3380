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
            var dataPoints1 = new List<DataPoint>();
            int[] CustomerNum = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var themeparkdbContext = _context.Transactions;

            foreach (Transactions t in themeparkdbContext)
            {
                if(t.DateOfSale.Year == 2018)
                {
                    Debug.Write("YEAHHHHH");
                    if (t.DateOfSale.Month == 1)
                    {
                        CustomerNum[0]++;
                        Debug.Write(CustomerNum[0]);
                    }
                    else if (t.DateOfSale.Month == 2)
                        CustomerNum[1]++;
                    else if (t.DateOfSale.Month == 3)
                        CustomerNum[2]++;
                    else if (t.DateOfSale.Month == 4)
                        CustomerNum[3]++;
                }
                else if(t.DateOfSale.Year == 2017)
                {
                    if (t.DateOfSale.Month == 12)
                        CustomerNum[11]++;
                    else if (t.DateOfSale.Month == 11)
                        CustomerNum[10]++;
                    else if (t.DateOfSale.Month == 10)
                        CustomerNum[9]++;
                    else if (t.DateOfSale.Month == 9)
                        CustomerNum[8]++;
                    else if (t.DateOfSale.Month == 8)
                        CustomerNum[7]++;
                    else if (t.DateOfSale.Month == 7)
                        CustomerNum[6]++;
                    else if (t.DateOfSale.Month == 6)
                        CustomerNum[5]++;
                    else if (t.DateOfSale.Month == 5)
                        CustomerNum[4]++;
                }
            }



            dataPoints1.Add(new DataPoint("Jan", CustomerNum[0]));
            dataPoints1.Add(new DataPoint("Feb", CustomerNum[1]));
            dataPoints1.Add(new DataPoint("Mar", CustomerNum[2]));
            dataPoints1.Add(new DataPoint("Apr", CustomerNum[3]));
            dataPoints1.Add(new DataPoint("May", CustomerNum[4]));
            dataPoints1.Add(new DataPoint("Jun", CustomerNum[5]));
            dataPoints1.Add(new DataPoint("Jul", CustomerNum[6]));
            dataPoints1.Add(new DataPoint("Aug", CustomerNum[7]));
            dataPoints1.Add(new DataPoint("Sep", CustomerNum[8]));
            dataPoints1.Add(new DataPoint("Oct", CustomerNum[9]));
            dataPoints1.Add(new DataPoint("Nov", CustomerNum[10]));
            dataPoints1.Add(new DataPoint("Dec", CustomerNum[11]));

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

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
