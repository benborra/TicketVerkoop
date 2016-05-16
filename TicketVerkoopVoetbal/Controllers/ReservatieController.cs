using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.DAO;
using Ticket.Service;
using Ticket.Model;
using System.Net;
using TicketVerkoopVoetbal.Models;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ReservatieController : Controller
    {

        WedstrijdService wedstrijdService;
        TicketService ticketService;
        StadionService stadionService;
        VakService vakService;

        public ReservatieController()
        {
            wedstrijdService = new WedstrijdService();
            ticketService = new TicketService();
            stadionService = new StadionService();
            vakService = new VakService();
        }
        // GET: Reservatie

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Wedstrijd");
            }

            Wedstrijd wedstrijd = wedstrijdService.Get(Convert.ToInt32(id));

            if (wedstrijd == null)
            {
                return HttpNotFound();
            }

            ViewBag.stadionNr = new SelectList(stadionService.All(), "StadionId", "stadionNaam");
            ViewBag.vakNummer = new SelectList(vakService.All(), "id", "naam");
            ViewBag.id = id;

            return View(wedstrijd);
        }

        [Authorize]
        public ActionResult Selected(int? id, FormCollection collection)
        {

            int aantalTickets = Convert.ToInt32(collection["AantalTickets"]);

            if (id != null && aantalTickets >= 1 && aantalTickets <= 10)
            {
                Plaats plaats = new Plaats();
                PlaatsService plaatService = new PlaatsService();
                StadionService stadionService = new StadionService();
                ClubService clubService = new ClubService();
                wedstrijdService = new WedstrijdService();
                vakService = new VakService();

                int VakId = Convert.ToInt32(collection["vakNummer"]);

                Wedstrijd wedstrijd = wedstrijdService.Get(Convert.ToInt16(id));
                plaats = plaatService.GetPlaatsPerVakAndStadion(Convert.ToInt32(collection["vakNummer"]), wedstrijd.stadionId);

                CartViewModel item = new CartViewModel
                {
                    WedstrijdId = wedstrijd.id,
                    ThuisPloeg = wedstrijd.thuisPloeg,
                    ThuisPloegNaam = clubService.Get(wedstrijd.thuisPloeg).naam,
                    BezoekersPloeg = wedstrijd.bezoekersPloeg,
                    BezoekersPloegNaam = clubService.Get(wedstrijd.bezoekersPloeg).naam,
                    Stadion = wedstrijd.stadionId,
                    StadionNaam = stadionService.Get(wedstrijd.stadionId).naam,
                    Datum = wedstrijd.Date,
                    Aantal = Convert.ToInt32(collection["AantalTickets"]),
                    Prijs = plaats.prijs,
                    Plaats = Convert.ToInt32(collection["vakNummer"]),
                    PlaatsNaam = vakService.getVak(plaats.Vakid).naam
                };

                ShoppingCartViewModel shopping;
                if (Session["ShoppingCart"] != null)
                {
                    
                    shopping = (ShoppingCartViewModel)Session["ShoppingCart"];
                    if (shopping.Cart != null)
                    {
                        shopping.Cart.Add(item);
                    }
                    else
                    {
                        shopping.Cart = new List<CartViewModel>();
                        shopping.Cart.Add(item);
                    }

                }
                else
                {

                    shopping = new ShoppingCartViewModel();
                    shopping.Cart = new List<CartViewModel>();
                    shopping.Cart.Add(item);
                }

                Session["ShoppingCart"] = shopping;

                return RedirectToAction("index", "ShoppingCart");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShoppingCartViewModel shopping = (ShoppingCartViewModel)Session["ShoppingCart"];
            ShoppingCartViewModel newlist = new ShoppingCartViewModel();
            newlist.Cart = new List<CartViewModel>();
            List<CartViewModel> cartList = shopping.Cart;
            foreach (CartViewModel cartview in cartList)
            {
                if (cartview.WedstrijdId != id)
                {
                    newlist.Cart.Add(cartview);
                }
            }
            Session["ShoppingCart"] = newlist;
            return RedirectToAction("index", "ShoppingCart");
        }
        
    }
}