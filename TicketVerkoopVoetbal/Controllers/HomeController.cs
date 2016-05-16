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
            // eerste 5 weedstrijden ophalen
            
            return View();
        }

        public ActionResult Stadions()
        {
            return View();
        }
    }
}