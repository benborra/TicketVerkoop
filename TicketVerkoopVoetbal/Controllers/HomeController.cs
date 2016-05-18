using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Controllers
{
    public class HomeController : Controller
    {
     

        public ActionResult Index()
        {
            ClubService s;
            WedstrijdService wed;
           

            // hieronder volgt een algoritme om de content aan te passen naar de gebruiker
            // indien hij niet ingelogd is, zal er gevraagd worden om in te loggen
            // indien hij nog niets heeft gekocht dan zal er staan "bekijk wedstrijden"
            // indien hij een abonnement heeft, zal die ploeg op de homepage staan
            // en anders zal de ploeg waarvan hij het meeste thuistickets heeft gekocht op de homepage staan
            // indien er een paar ploegen zijn waar er evenveel thuistickets van zijn, wordt daar random uit gekozen
            var userid = User.Identity.GetUserId();
            // is user ingelogd
            if (userid == null)
            {
                ViewBag.ClubNaam = null;
            }
            else
            {
                // Abonnement heeft voorrang op tickets
                AbonnementService abboService = new AbonnementService();
                Abonnement ab = abboService.GetFromUserId(userid);
                if (ab != null)
                {
                    s = new ClubService();
                    Clubs cl = s.Get(ab.Clubsid);
                    ViewBag.ClubNaam = cl.naam;
                    ViewBag.Image = cl.logo;
                }
                else
                {
                    // tickets checken
                    TicketService t = new TicketService();
                    IEnumerable<Tickets> ticks = t.getTicketsperPersoon(userid);
                    Dictionary<int, int> Lijst = new Dictionary<int, int>();
                    // er moeten tickets zijn, anders is hij helemaal nieuw
                    if (ticks.Count() > 1)
                    {
                        foreach (Tickets ticket in ticks)
                        {
                            // kijken naar welke ploeg hij al het meest thuis gaan kijken is
                            s = new ClubService();
                            wed = new WedstrijdService();
                            Wedstrijd wedstrijd = wed.Get(ticket.Wedstrijdid);
                            Clubs club = s.Get(wedstrijd.thuisPloeg);
                            if (Lijst.ContainsKey(club.id))
                            {
                                //  1 keer bijtellen
                                Lijst[club.id] += 1;
                            }
                            else
                            {
                                // toevoegen en een eerste waarde geven
                                Lijst.Add(club.id, 1);
                            }
                            // wij hebben nu een lijst met aantal keer dat thuisploeg voorkomt, nu kijken welke ploeg hij al het meest heeft bezocht
                            int maxValue = 0;
                            foreach (KeyValuePair<int, int> entry in Lijst)
                            {
                                if (entry.Value > maxValue)
                                {
                                    maxValue = entry.Value;
                                }
                            }
                            // we hebbenn nu maxvalue, kan zijn dat er een 2 of meerdere ploegen dezelfde waarde hebben
                            // var clubIDs = Lijst.FirstOrDefault(x => x.Value == maxValue).Key;
                            var clubIDs = Lijst.Where(item => item.Value == maxValue).Select(item => item.Key).ToList();
                            if (clubIDs.Count() == 1)
                            {
                                // enigste waarde
                                club = s.Get(clubIDs[0]);
                                ViewBag.ClubNaam = club.naam;
                                ViewBag.Image = club.logo;
                            }
                            else
                            {
                                // kies een random
                                Random rnd = new Random();
                                int lucky = rnd.Next(clubIDs.Count() -1);
                                club = s.Get(clubIDs[lucky]);
                                ViewBag.ClubNaam = club.naam;
                                ViewBag.Image = club.logo;
                            }
                        }
                    }
                    else
                    {
                        // ingelogd, geen abbo, geen tickets #NOOB
                        // toon standaard "zie wedstrijden"
                        ViewBag.ClubNaam = "newuser";
                    }
                }
            }
            return View();
        }

        public ActionResult Stadions()
        {
            return View();
        }
    }
}