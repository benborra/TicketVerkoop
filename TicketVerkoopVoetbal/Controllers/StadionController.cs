using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticket.Model;
using Ticket.Service;

namespace TicketVerkoopVoetbal.Controllers
{
    public class StadionController : Controller
    {

        private StadionService stadionService;
        // GET: Stadion
        public ActionResult Index()
        {
            stadionService = new StadionService();
            var stadions = stadionService.All();
            if (stadions == null)
            {
                return HttpNotFound();
            }
            // in Beers staat dit zo , maar in de view wordt er gelijk niets mee gedaan
            // wat is het verschil tussen model.Naam en de viewbag?
            ViewBag.stadions =
                   new SelectList(stadionService.All(), "naam", "adres");
            return View(stadions);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            stadionService = new StadionService();
            Stadion stadion = stadionService.Get(Convert.ToInt32(id));
            if (stadion == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.StadionNr =
                    new SelectList(stadionService.All(), "Brouwernr", "Naam");

            return View(stadion);
        }

        public ActionResult Save()
        {
            // als er op de button save wordt geklikt komen we hiernaartoe
            // insert update statement met controle
            // terugkeren naar de lijst, die zal geupdate zijn.
            return RedirectToAction("Index");
        }
    }
}