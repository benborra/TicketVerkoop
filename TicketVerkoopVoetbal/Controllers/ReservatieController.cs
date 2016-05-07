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
        VakService vakService;
        public ReservatieController()
        {
            wedstrijdService = new WedstrijdService();
            ticketService = new TicketService();
            vakService = new VakService();
        }
        // GET: Reservatie

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Wedstrijd");
            }
   
            ViewBag.WedstrijdId = wedstrijdService.Get(Convert.ToInt16(id));
            // meegeven van dropdownlist zodat er kan gekozen worden in welk vak ze willen zitten
            ViewBag.vakLijst =
                     new SelectList(vakService.All(), "id", "naam");
            // TODO op de pagina zetten we een save button die enkel getoond word indien de gebruiker ingelogd is
            // indien niet ingelogd => doorverwijzen naar inlogpagina

            return View();
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