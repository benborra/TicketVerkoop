using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketVerkoopVoetbal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ticket verkoop voetbal.";

            return View();
        }

        public ActionResult Stadions()
        {
            ViewBag.Message = "Hier komen , dynamisch geladen weliswaar, alle stadions";

            return View();
        }
    }
}