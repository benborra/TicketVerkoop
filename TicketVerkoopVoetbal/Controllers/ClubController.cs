using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Ticket.Model;
using Ticket.Service;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ClubController : Controller
    {
        private ClubService clubService;


        public ClubController()
        {
            clubService = new ClubService();
        }

        public ActionResult Index()
        {
            var clubs = clubService.All();
            return View(clubs);
        }

        // GET: Club/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            clubService = new ClubService();
            Clubs clubs = clubService.Get(Convert.ToInt32(id));

            if (clubs == null)
            {
                return HttpNotFound();
            }

            return View(clubs);
        }

        // GET: Club/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Club/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                clubService = new ClubService();
                Clubs club = new Clubs();

                club.naam = collection["naam"];
                club.Stadionid = Convert.ToInt32(collection["StadionId"]);
                club.logo = collection["logo"];

                clubService.Add(club);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Club/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            clubService = new ClubService();
            Clubs clubs = clubService.Get(Convert.ToInt32(id));

            if (clubs == null)
            {
                return HttpNotFound();
            }

            StadionService stadionService = new StadionService();
            ViewBag.StadionId = new SelectList(stadionService.All(), "Stadion nr", "Naam");

            return View(clubs);
        }

        // POST: Club/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(
            [Bind(Include = "Id, Naam, StadionId, Logo")]Clubs entity, string stadionId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    clubService = new ClubService();
                    clubService.Update(entity);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("DataExeption", "Exception in wegschrijven edit: " + ex);
            }


            return View(entity);
        }

        // GET: Club/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            clubService = new ClubService();
            Clubs clubs = clubService.Get(Convert.ToInt32(id));

            if (clubs == null)
            {
                return HttpNotFound();
            }

            StadionService stadionService = new StadionService();
            ViewBag.StadionId = new SelectList(stadionService.All(), "Stadion nr", "Naam");

            return View(clubs);
        }

        // POST: Club/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Clubs club = clubService.Get(id);
                clubService.RemoveClub(club);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
