using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ticket.Model;
using Ticket.Service;
using TicketVerkoopVoetbal.Models;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ShoppingcartController : Controller
    {
        // GET: Shoppingcart
        public ActionResult Index()
        {

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];

            return View(cartList);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];

            var itemToRemove = cartList.Cart.FirstOrDefault(r => r.WedstrijdId == id);
            if (itemToRemove != null)
            {
                cartList.Cart.Remove(itemToRemove);
                Session["shoppingCart"] = cartList;
            }


            return View("index", cartList);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Payment()
        {
            UserService userService = new UserService();
            var user = userService.Get(User.Identity.Name);

            //try
            //{
                ShoppingCartViewModel shopping = (ShoppingCartViewModel)Session["ShoppingCart"];
                CartViewModel cart = new CartViewModel();

                WedstrijdService wedstrijdService = new WedstrijdService();
                PlaatsService plaatsService = new PlaatsService();
                StadionService stadionService = new StadionService();
                TicketService ticketService = new TicketService();
                List<int> tickets = new List<int>();
                Tickets ticket = new Tickets();

                for (int i = 0; i < shopping.Cart.Count; i++)
                {
                    cart = shopping.Cart[i];
                    int ticketsBeschikbaar = plaatsService.GetPlaats(cart.Plaats).aantal - ticketService.getTicketsPerWedstrijdPerVak(cart.WedstrijdId, cart.Plaats);

                    if (ticketsBeschikbaar >= cart.Aantal)
                    {
                        ticket.Persoonid = user.Id;
                        ticket.Wedstrijdid = cart.WedstrijdId;
                        ticket.plaatsId = cart.Plaats;
                        ticket.Betaald = true;

                        // Ticket toevoegen aan db
                        ticketService.Add(ticket);

                        // Ticket ID in lijst stoppen zodanig deze later terug opgehaald kan worden om aan mail toe te voegen
                        tickets.Add(ticket.id);
                    }
                    else
                    {
                        // todo: Wanneer laatste moment iets uitverkocht graakt terwijl bestelling afgerond wordt
                        return RedirectToAction("index", "ShoppingCart");
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    return RedirectToAction("Index", "Error");
            //}

            return View();
        }
    }
}