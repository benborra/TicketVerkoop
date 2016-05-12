using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticket.Service;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Controllers
{
    public class WinkelwagenController : Controller
    {

        WedstrijdService wedstrijdService;

        public ActionResult Winkelwagen()
        {
            return View();
        }

        [Authorize]
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.Get(Convert.ToInt16(id));

            Tickets item = new Tickets()
            {
                barcode = 0, // TODO Barcode genereren
                Wedstrijdid = wedstrijd.id,
                Persoonid = null, // TODO add iser id to ticket
                plaatsId = 0, // TODO plaats meegeven van ticket
                Betaald = false
            };

            //ShoppingCartViewModel shopping;
            //if (Session["ShoppingCart"] != null)
            //{
            //    shopping = (ShoppingCartViewModel)Session["ShoppingCart"];
            //    shopping.Cart.Add(item);
            //}
            //else
            //{
            //    shopping = new ShoppingCartViewModel();
            //    shopping.Cart = new List<CartViewModel>();
            //    shopping.Cart.Add(item);
            //}




            //Session["ShoppingCart"] = shopping;
            //return RedirectToAction("Index", "ShoppingCart");
            return null;

        }
    }
}