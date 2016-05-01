using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Club/Create
        [Authorize(Roles="Admin")]
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
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Club/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Club/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Club/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Club/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
