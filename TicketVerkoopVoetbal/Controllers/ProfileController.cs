using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Ticket.Model;
using Ticket.Service;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ProfileController : Controller
    {
        UserService userService;
        TicketService ticketService;
        WedstrijdService wedstrijdService;
        ClubService clubService;
        PlaatsService plaatsService;
        VakService vakService;

        [Authorize]
        public new ActionResult View()
        {
            userService = new UserService();
            ticketService = new TicketService();
            wedstrijdService = new WedstrijdService();
            clubService = new ClubService();
            plaatsService = new PlaatsService();
            vakService = new VakService();

            Dictionary<int, Wedstrijd> wedstrijden = new Dictionary<int, Wedstrijd>();
            Dictionary<int, Wedstrijd> wedstrijdenOld = new Dictionary<int, Wedstrijd>();
            Dictionary<int, string> clubLogo = new Dictionary<int, String>();
            Dictionary<int, string> plaatsNaam = new Dictionary<int, string>(); //int geet plaatsId, plaats geeft vaknaam waarin ticket geldig is
            var user = userService.Get(User.Identity.Name);

            var tickets = ticketService.getTicketsperPersoon(user.Id);

            foreach (var ticket in tickets)
            {
                if (!(wedstrijden.ContainsKey(ticket.Wedstrijdid) || wedstrijdenOld.ContainsKey(ticket.Wedstrijdid)))
                {
                    var wedstrijd = wedstrijdService.Get(ticket.Wedstrijdid);
                    if (wedstrijd.Date >= DateTime.Now) wedstrijden.Add(ticket.Wedstrijdid, wedstrijdService.Get(ticket.Wedstrijdid));
                    else wedstrijdenOld.Add(ticket.Wedstrijdid, wedstrijdService.Get(ticket.Wedstrijdid));
                }

                if (!(plaatsNaam.ContainsKey(ticket.plaatsId)))
                {
                    // haalt plaats op doormiddel van id, gebruikt het ID hiervan om het vak te achterhalen, die de naam terug geeft van het vak
                    plaatsNaam.Add(ticket.plaatsId, vakService.getVak(plaatsService.GetPlaats(ticket.plaatsId).id).naam);
                }
            }

            foreach (int id in wedstrijden.Keys)
            {
                Wedstrijd wedtr = wedstrijden[id];

                if (!(clubLogo.ContainsKey(wedtr.thuisPloeg)))
                {
                    clubLogo.Add(wedtr.thuisPloeg, clubService.GetClubLogo(wedtr.thuisPloeg));
                }

                if (!(clubLogo.ContainsKey(wedtr.bezoekersPloeg)))
                {
                    clubLogo.Add(wedtr.bezoekersPloeg, clubService.GetClubLogo(wedtr.bezoekersPloeg));
                }
            }

            foreach (int id in wedstrijdenOld.Keys)
            {
                Wedstrijd wedtr = wedstrijdenOld[id];

                if (!(clubLogo.ContainsKey(wedtr.thuisPloeg)))
                {
                    clubLogo.Add(wedtr.thuisPloeg, clubService.GetClubLogo(wedtr.thuisPloeg));
                }

                if (!(clubLogo.ContainsKey(wedtr.bezoekersPloeg)))
                {
                    clubLogo.Add(wedtr.bezoekersPloeg, clubService.GetClubLogo(wedtr.bezoekersPloeg));
                }
            }

            ViewBag.Wedstrijden = wedstrijden;
            ViewBag.WedstrijdenOld = wedstrijdenOld;
            ViewBag.Tickets = tickets;
            ViewBag.ClubLogo = clubLogo;
            ViewBag.PlaatsNaam = plaatsNaam;

            return View(user);
        }
    }
}