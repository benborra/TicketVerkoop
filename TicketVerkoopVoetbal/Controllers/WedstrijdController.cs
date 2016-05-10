﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;
using System.Net;

namespace TicketVerkoopVoetbal.Controllers
{
    public class WedstrijdController : Controller
    {
        WedstrijdService wedstrijdService;
        ClubService clubService;
        TicketService ticketService;
        StadionService stadionService;
        
        public WedstrijdController()
        {
            wedstrijdService = new WedstrijdService();
            clubService = new ClubService();
            ticketService = new TicketService();
            stadionService = new StadionService();
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
            ViewBag.AantalTickets = Convert.ToString(ticketService.getTicketsPerWedstrijd(w).Count());

            Stadion stad = stadionService.Get(w.stadionId);
            //TODO meegeven hoeveel tickets er nog te verkrijgen zijn

            return View(w);
        }

        

        public ActionResult New()
        {
            ViewBag.TPloegen =
                new SelectList(clubService.All(), "id", "naam");
            ViewBag.BPloegen =
               new SelectList(clubService.All(), "id", "naam");

            ViewBag.Stadion =
                new SelectList(stadionService.All(), "id", "naam");
            return View();
        }
        // TODO: datepicker layout niet optimaal
        // TODO: Controle of de ploegen gelijk zijn
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                
                Wedstrijd wedstrd = new Wedstrijd();

                wedstrd.stadionId = Convert.ToInt32(collection["Stadion"]);
                wedstrd.thuisPloeg = Convert.ToInt32(collection["TPloegen"]);
                wedstrd.bezoekersPloeg = Convert.ToInt32(collection["BPloegen"]);
                wedstrd.Date = Convert.ToDateTime(collection["Date"]);
                wedstrijdService.AddWedstrijd(wedstrd);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult FilterDate(DateTime date)
        {

            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.WedstrijdDatum =
                    new SelectList(wedstrijdService.NogTeSpelenWedstrijden(), "Date", "Date");
            ViewBag.Ploegen =
                new SelectList(clubService.All(), "id", "naam");
            var list = wedstrijdService.WedstrijdPerDatum(date);
            return View(list);
        }

        [HttpPost]
        public ActionResult FilterPloeg(int? ploegId)
        {

            if (ploegId == null)
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