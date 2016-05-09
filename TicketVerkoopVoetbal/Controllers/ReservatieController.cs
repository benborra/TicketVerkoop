using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.DAO;
using Ticket.Service;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ReservatieController : Controller
    {

        WedstrijdService wedstrijdService;
        TicketService ticketService;
        StadionService stadionService;
        VakService vakService;

        public ReservatieController()
        {
            wedstrijdService = new WedstrijdService();
            ticketService = new TicketService();
            stadionService = new StadionService();
            vakService = new VakService();
        }
        // GET: Reservatie

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Wedstrijd");
            }

            Wedstrijd wedstrijd = wedstrijdService.Get(Convert.ToInt32(id));

            if (wedstrijd == null)
            {
                return HttpNotFound();
            }

            ViewBag.stadionNr = new SelectList(stadionService.All(), "StadionId", "stadionNaam");
            ViewBag.vakNummer = new SelectList(vakService.All(), "id", "naam");

            return View(wedstrijd);
        }
   
        public ActionResult ReserveerConfirmed(Tickets ticket)
        {
            // TODO functie om ticket op te slaan
            ticketService.Add(ticket);
            // redirect naar ... ?
            return RedirectToAction("Index");
        }

    }
}