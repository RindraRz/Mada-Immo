using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mada_immo.Models.Csv;
using Mada_immo.Models.Data;
using Mada_immo.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mada_immo.Controllers
{
    [TypeFilter(typeof(SessionAdminFilter))]
    public class AdminController : Controller
    {

        private readonly ImmoContext _context;

        public AdminController(ImmoContext context)
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

            List<StatistiqueGain> statistiqueGains = Admin.GetStatistiqueGainsEntreDeuxDates(_context, d1, d2);
            var retour = new
            {
                Date1 = d1,
                Date2 = d2,
                StatistiqueGains = statistiqueGains,
                TotalChiffre = statistiqueGains.Sum(c => c.ChiffreAffaire),
                TotalGain = statistiqueGains.Sum(c => c.Gain)

            };
            return View(retour);

        }
        public IActionResult Reset()
        {
            SessionHelper.ResetDataBase(_context);
            return Ok("Base videe");
        }
        public ActionResult ImportDonne()
        {
            return View();
        }

        public async Task<IActionResult> ExecuteImportDonne(IFormFile commission, IFormFile bien, IFormFile location)
        {
            try
            {

                // le header efa tsy ao anatin le donnees intsony
                string delimiter = ",";
                List<string[]> commissions = new List<string[]>();
                List<string[]> biens = new List<string[]>();
                List<string[]> locations = new List<string[]>();
                if (commission != null && commission.Length > 0)
                {
                    string fileName = Path.GetFileName(commission.FileName);
                    using (var stream = new MemoryStream())
                    {
                        commission.CopyTo(stream);

                        commissions = SessionHelper.GetDataFromCsvFile(fileName, delimiter);
                    }
                }

                bool c = CommissionCsv.Insert(_context, commissions);
                if (bien != null && bien.Length > 0)
                {
                    string fileName = Path.GetFileName(bien.FileName);
                    using (var stream = new MemoryStream())
                    {
                        bien.CopyTo(stream);

                        biens = SessionHelper.GetDataFromCsvFile(fileName, delimiter);
                    }
                }

                bool t = BienCsv.Insert(_context, biens);

                if (location != null && location.Length > 0)
                {
                    string fileName = Path.GetFileName(location.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await location.CopyToAsync(stream);

                        locations = SessionHelper.GetDataFromCsvFile(fileName, delimiter);
                    }


                }
                bool l = LocationCsv.Insert(_context, locations);
                if (c && t && l) TempData["message"] = "Import reussi";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["erreur"] = e.Message;
            }
            return RedirectToAction("ImportDonne", "Admin");
        }

        public IActionResult AjoutLocation()
        {
            var retour = new
            {
                Clients = Client.GetAll(_context),
                Biens = Bien.GetAll(_context)
            };
            return View(retour);
        }

        public IActionResult ExecuteAjoutLocation(string client, string duree, string date_debut, string bien)
        {
            Location location = new Location();

            location.ClientId = int.Parse(client);
            location.Duree = int.Parse(duree);
            location.DateDebut = DateOnly.Parse(date_debut);
            location.BienId = int.Parse(bien);


            try
            {

                DateOnly disponible = Bien.GetDateDisponibilite(_context, location.BienId);
                if (location.DateDebut < disponible)
                {
                    throw new Exception("Bien indisponible . Disponible qu au " + disponible);
                }
                Admin.AddLocation(_context, location);
                TempData["message"] = "Insertion reussi";
            }
            catch (Exception e)
            {
                TempData["erreur"] = e.Message;
            }


            return RedirectToAction("AjoutLocation", "Admin");
        }

        public IActionResult Locations()
        {
            List<Location> locations = Admin.GetAllLocations(_context);

            var retour = new
            {
                Locations = locations
            };
            return View(retour);
        }

        public IActionResult Detail(string idLocation)
        {
            List<DetailLocation> locations = Admin.GetDetailLocation(_context, int.Parse(idLocation));
            var retour = new
            {
                Details = locations
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