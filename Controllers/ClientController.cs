using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mada_immo.Models.Data;
using Mada_immo.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mada_immo.Controllers
{
    [TypeFilter(typeof(SessionClientFilter))]
    public class ClientController : Controller
    {
        private readonly ImmoContext _context;

        public ClientController(ImmoContext context)
        {
            _context = context;
        }


        public IActionResult Index(string? date1, string? date2)
        {
            DateOnly d1 = new DateOnly();
            DateOnly d2 = new DateOnly();
            if (date1 == null && date2 == null)
            {
                d1 = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                d2 = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                d1 = DateOnly.Parse(date1);
                d2 = DateOnly.Parse(date2);
            }
            int? id = HttpContext.Session.GetInt32("idClient");
            List<DetailLocation> locations = Client.GetLoyerEntreDeuxDates(_context, (int)id, d1, d2);
            var retour = new
            {
                Date1 = d1,
                Date2 = d2,
                Locations = locations
            };
            return View(retour);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}