using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;
using System.Net;
using System.Globalization;

namespace TicketVerkoopVoetbal.Controllers
{
    public class WedstrijdController : Controller
    {
        WedstrijdService wedstrijdService;
        ClubService clubService;
        TicketService ticketService;
        StadionService stadionService;
        VakService vakService;
        AbonnementService abboService;

        public WedstrijdController()
        {
            wedstrijdService = new WedstrijdService();
            clubService = new ClubService();
            ticketService = new TicketService();
            stadionService = new StadionService();
            vakService = new VakService();
            abboService = new AbonnementService();
            
        }

        // GET: Wedstrijd
        public ActionResult Index()
        {
            var list = wedstrijdService.NogTeSpelenWedstrijden();
            wedstrijdService = new WedstrijdService();
            ViewBag.WedstrijdDatum =
                    new SelectList(wedstrijdService.NogTeSpelenWedstrijden(), "Date", "Date");
            ViewBag.Ploegen =
                new SelectList(clubService.All(), "id", "naam");
            return View(list);
        }

        public ActionResult Details(int id)
        {
            Wedstrijd w = wedstrijdService.Get(id);
            //aantal aanwezigen
            // tickets die nog in abonnemlenten zitten

            int abboTickets = abboService.getFromClubdId(w.thuisPloeg).Count();

            int numberOfTickets = ticketService.getTicketsPerWedstrijd(id).Count();
            numberOfTickets += abboTickets;
            ViewBag.AantalTickets = Convert.ToString(numberOfTickets);
            // meegeven hoeveel tickets er nog te verkrijgen zijn
            Stadion stad = stadionService.Get(w.stadionId);
            ViewBag.AantalTicketsBeschikbaar = Convert.ToString(vakService.getAantalZitPlaatsenPerStadion(stad) - numberOfTickets);
            ViewBag.TicketSold = Convert.ToString(numberOfTickets);

            return View(w);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            ViewBag.TPloegen =
                new SelectList(clubService.All(), "id", "naam", 1);
            ViewBag.BPloegen =
               new SelectList(clubService.All(), "id", "naam", 2);

            ViewBag.Stadion =
                new SelectList(stadionService.All(), "id", "naam");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create(FormCollection collection)
        {

            Wedstrijd wedstrd = new Wedstrijd();
            CultureInfo us = new CultureInfo("en-US");

            wedstrd.stadionId = Convert.ToInt32(collection["Stadion"]);
            wedstrd.thuisPloeg = Convert.ToInt32(collection["TPloegen"]);
            wedstrd.bezoekersPloeg = Convert.ToInt32(collection["BPloegen"]);
            wedstrd.Date = DateTime.Parse(collection["Date"], us, System.Globalization.DateTimeStyles.AssumeLocal); 

            wedstrijdService.AddWedstrijd(wedstrd);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            Wedstrijd w = wedstrijdService.Get(Convert.ToInt32(id));

            ViewBag.TPloegen =
                new SelectList(clubService.All(), "id", "naam", w.thuisPloeg);
            ViewBag.BPloegen =
               new SelectList(clubService.All(), "id", "naam", w.bezoekersPloeg);

            ViewBag.Stadion =
                new SelectList(stadionService.All(), "id", "naam",w.stadionId);
            ViewBag.Date = w.Date;
            ViewBag.WedstrijdId = w.id;
            return View(w);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int? id, FormCollection collection)
        {

            Wedstrijd wedstrd = wedstrijdService.Get(Convert.ToInt32(id)); ;
            CultureInfo us = new CultureInfo("en-US");
            
            wedstrd.stadionId = Convert.ToInt32(collection["Stadion"]);
            wedstrd.thuisPloeg = Convert.ToInt32(collection["TPloegen"]);
            wedstrd.bezoekersPloeg = Convert.ToInt32(collection["BPloegen"]);
            wedstrd.Date = DateTime.Parse(collection["Date"], us, System.Globalization.DateTimeStyles.AssumeLocal);

            wedstrijdService.UpdateWedsrijd(wedstrd);
            return RedirectToAction("Index");
        }

        // TODO fix diz shat
        [HttpPost]
        public ActionResult FilterPloeg(FormCollection collection)
        {

            int ploegId = Convert.ToInt32(collection["Ploegen"]);
            if (ploegId < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.WedstrijdDatum =
                    new SelectList(wedstrijdService.NogTeSpelenWedstrijden(), "Date", "Date");
            ViewBag.Ploegen =
                new SelectList(clubService.All(), "id", "naam");
            var list = wedstrijdService.GetWedStrijdPerPloeg(Convert.ToInt16(ploegId));
            return View(list);
        }

        public ActionResult Geschiedenis()
        {
            var gespeeld = wedstrijdService.GespeeldeWedstrijden();
            return View(gespeeld);
        }

    }
}