using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;
using TicketVerkoopVoetbal.Models;

namespace TicketVerkoopVoetbal.Controllers
{
    public class AbonnementController : Controller
    {

        ClubService clubService;
        VakService vakService;
        SeizoenService seizoenService;
        AbonnementService abboService;
        PlaatsService plaatsService;

        public AbonnementController()
        {
            clubService = new ClubService();
            vakService = new VakService();
            seizoenService = new SeizoenService();
            abboService = new AbonnementService();
            plaatsService = new PlaatsService();
        }
        // GET: Abonnement
        public ActionResult Index()
        {
            // kiezen voor welke ploeg hij een abonnement wil kopen
            ViewBag.Ploegen =
               new SelectList(clubService.All(), "id", "naam");
            // Welke plaats hij wil 
            ViewBag.Plaatsen = new SelectList(vakService.All(), "id", "naam");
            
            ViewBag.Seizoen = new SelectList(seizoenService.All(), "id", "SeizoenString");

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            
            //// geeft 2015, 2016,.. terug
            // TODO: heeft deze persoon al een abonnement?
            // month is september, dan is de competitie al gestart, in september kunnen ze nog kopen
            if (Convert.ToInt32(collection["Seizoen"]) <= DateTime.Now.Year && DateTime.Now.Month > 9)
            {
                return RedirectToAction("Index");
            }
            Clubs c = clubService.Get(Convert.ToInt32(collection["Ploegen"]));
            Plaats p = plaatsService.GetPlaats(Convert.ToInt32(collection["Plaatsen"]));
            int prijs = Convert.ToInt32(p.prijs * 15);
            Vak v = vakService.getVak(p.Vakid);
            AbonnementViewModel abbo = new AbonnementViewModel
            {
                Seizoenid = Convert.ToInt32(collection["Seizoen"]),
                ClubsId = Convert.ToInt32(collection["Ploegen"]),
                Club = c.naam,
                PlaatsId = Convert.ToInt32(collection["Plaatsen"]),
                Plaats = v.naam,
                Prijs = prijs
            };

            ShoppingCartViewModel abbonement;
            
            if (Session["ShoppingCart"] != null)
            {
                abbonement = (ShoppingCartViewModel)Session["ShoppingCart"];
                if (abbonement.abbo != null)
                {
                    // er is al een abonnement
                    return RedirectToAction("index", "ShoppingCart");
                }
                abbonement.abbo = abbo;

            }
            else
            {
                abbonement = new ShoppingCartViewModel();
                abbonement.abbo = abbo;
            }

            Session["ShoppingCart"] = abbonement;

            return RedirectToAction("index", "ShoppingCart");
           
            
        }
    }
}