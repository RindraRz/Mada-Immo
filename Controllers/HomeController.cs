using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mada_immo.Models;
using Mada_immo.Models.Data;

namespace Mada_immo.Controllers;

public class HomeController : Controller
{
    private readonly ImmoContext _context;

    public HomeController(ImmoContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        DateOnly date1 = new DateOnly(2024, 02, 01);


        DateOnly dateDebut = new DateOnly(2024, 1, 31); // 1er juillet 2024
        DateOnly date2 = new DateOnly(dateDebut.Year, dateDebut.Month, 1);
        int duree = 1; // Par exemple, pour ajouter 2 mois

        // Calcul de la date_fin
        DateOnly dateFin = date2.AddMonths(duree).AddDays(-1);
        return Ok(dateFin.ToString());
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AdminLog()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAdmin(Admin model)
    {
        // if(!ModelState.IsValid){

        //     return View("Index",model);
        // }
        try
        {
            Admin? admin = Admin.LogIn(_context, model.Email, model.Mdp);
            if (admin == null)
            {
                TempData["login"] = "email ou mot de passe incorrect";
                return RedirectToAction("AdminLog", "Home");
            }

            HttpContext.Session.SetInt32("idAdmin", admin.AdminId);

            return RedirectToAction("Index", "Admin");
        }
        catch (Exception e)
        {
            TempData["login"] = e.Message;
            return RedirectToAction("AdminLog", "Home");
        }

    }

    public IActionResult Prop()
    {

        return View("PropLog");
    }

    [HttpPost]
    public async Task<IActionResult> LoginProp(Proprietaire model)
    {
        if (!ModelState.IsValid)
        {
            return View("PropLog", model);
        }
        try
        {
            Proprietaire proprietaire = Proprietaire.LogIn(_context, model.Contact);



            HttpContext.Session.SetInt32("idProp", proprietaire.ProprietaireId);
            HttpContext.Session.SetString("contact", proprietaire.Contact);


            return RedirectToAction("Index", "Proprietaire");
        }
        catch (Exception e)
        {
            TempData["login"] = e.Message;
            return RedirectToAction("Prop", "Home");
        }
    }
    public async Task<IActionResult> LogOutAdmin()
    {
        HttpContext.Session.Remove("idAdmin");

        return RedirectToAction("AdminLog", "Home");
    }
    public async Task<IActionResult> LogOutProp()
    {
        HttpContext.Session.Remove("idProp");
        HttpContext.Session.Remove("contact");

        return RedirectToAction("Prop", "Home");
    }

    public IActionResult Clients()
    {

        return View("Client");
    }


    [HttpPost]
    public async Task<IActionResult> LoginClient(Client model)
    {
        if (!ModelState.IsValid)
        {
            return View("Client", model);
        }
        try
        {
            Client client = Client.LogIn(_context, model.Email);



            HttpContext.Session.SetInt32("idClient", client.ClientId);



            return RedirectToAction("Index", "Client");
        }
        catch (Exception e)
        {
            TempData["login"] = e.Message;
            return RedirectToAction("Clients", "Home");
        }
    }

    public async Task<IActionResult> LogOutClient()
    {
        HttpContext.Session.Remove("idClient");


        return RedirectToAction("Clients", "Home");
    }





}
