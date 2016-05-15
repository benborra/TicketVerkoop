using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Controllers
{
    public class AbonnementController : Controller
    {

        ClubService clubService;
        VakService vakService;
        SeizoenService seizoenService;
        AbonnementService abboService;

        public AbonnementController()
        {
            clubService = new ClubService();
            vakService = new VakService();
            seizoenService = new SeizoenService();
            abboService = new AbonnementService();
        }
        // GET: Abonnement
        public ActionResult Index()
        {
            // kiezen voor welke ploeg hij een abonnement wil kopen
            ViewBag.Ploegen =
               new SelectList(clubService.All(), "id", "naam");
            // Welke plaats hij wil 
            ViewBag.Plaatsen = new SelectList(vakService.All(), "id", "naam");

            ViewBag.Seizoen = new SelectList(seizoenService.All(), "id", "seizoenString");

            return View();
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
             Abonnement abbo = new Abonnement();
            // get persoon die ingelogt is, hoe
           // abbo.Persoonid = Convert.ToInt32(collection["Stadion"]);
            abbo.PlaatsId = Convert.ToInt32(collection["Plaatsen"]);
            abbo.Seizoenid = Convert.ToInt32(collection["Seizoen"]);
            abbo.Clubsid = Convert.ToInt32(collection["Ploegen"]);

            abboService.Add(abbo);

            // TODO: redirect naar winkelmandje ?
            return View();
        }
    }
}