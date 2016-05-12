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
        private ClubService clubService;
        // GET: Stadion

        public StadionController() {
            stadionService = new StadionService();
        }
        public ActionResult Index()
        {
            
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

            
            Stadion stadion = stadionService.Get(Convert.ToInt32(id));
            if (stadion == null)
            {
                return HttpNotFound();
            }


            ViewBag.StadionNr =
                    new SelectList(stadionService.All(), "Brouwernr", "Naam");

            return View(stadion);
        }

        // details
        public ActionResult Details(int? id)
        {
            Stadion s = stadionService.Get(Convert.ToInt32(id));
            // opzoeken van welk team dit stadion is
            // om mee te geven 
            
            return View(s);
        }

        [HttpPost]
        public ActionResult UpdateStadion(Stadion stadion)
        {
            if (stadion == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            stadionService.UpdateStadion(stadion);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            // controle of het stadion in gebruik is, dan kan je niet verwijderen
            Stadion s = stadionService.Get(id);
            stadionService.RemoveStadion(s);

            return RedirectToAction("Index");
        }

        // klikt actionLink
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Stadion stadion)
        {
            stadionService.AddStadion(stadion);
            return RedirectToAction("Index");
        }
    }
}