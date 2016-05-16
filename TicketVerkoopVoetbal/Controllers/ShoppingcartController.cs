using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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

            ShoppingCartViewModel cartListAll = (ShoppingCartViewModel)Session["shoppingCart"];
            //TODO error fixxen 
            AbonnementViewModel m = (AbonnementViewModel)cartListAll.abbo;
            if (m != null)
            {
                ViewBag.Ploeg = m.Club;
                ViewBag.Plaats = m.Plaats;
                ViewBag.Prijs = m.Prijs;
                
            }
            return View(cartListAll);
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

        public ActionResult DeleteAbbo()
        {

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];
            cartList.abbo = null;
            Session["ShoppingCart"] = cartList;
            return RedirectToAction("index", "ShoppingCart");
        }


        [Authorize]
        [HttpPost]
        public ActionResult Payment()
        {
            UserService userService = new UserService();
            var user = userService.Get(User.Identity.Name);
            int maxTickets = Convert.ToInt32(ConfigurationManager.AppSettings["maxTicket"]);


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
                int countTicket = ticketService.GetTicketsPerPersoonPerWedstrijd(user.Id, cart.WedstrijdId).Count();

                if (ticketsBeschikbaar >= cart.Aantal)
                {
                    if ((countTicket + cart.Aantal) <= maxTickets)
                    {
                        for (int j = 0; j < cart.Aantal; j++)
                        {
                            ticket.Persoonid = user.Id;
                            ticket.Wedstrijdid = cart.WedstrijdId;
                            ticket.plaatsId = cart.Plaats;
                            ticket.Betaald = true;

                            Boolean unique = false;
                            long barcode = -1;

                            while (!unique)
                            {
                                Random r = new Random();
                                int bar = r.Next(100000000, 999999999);

                                barcode = Convert.ToInt64(bar.ToString() + ticket.Wedstrijdid.ToString() + ticket.plaatsId.ToString());

                                if (ticketService.ZoekTicketBarcode(barcode) == 0) unique = true;
                            }

                            ticket.barcode = barcode;

                            // Ticket toevoegen aan db
                            ticketService.Add(ticket);

                            // Ticket ID in lijst stoppen zodanig deze later terug opgehaald kan worden om aan mail toe te voegen
                            tickets.Add(ticket.id);
                        }
                    }
                    else
                    {
                        TempData["Tickets"] = countTicket;
                        TempData["Thuis"] = cart.ThuisPloegNaam;
                        TempData["Bezoek"] = cart.BezoekersPloegNaam;
                        return RedirectToAction("index", "ShoppingCart");
                    }
                }
                else
                {
                    

                    TempData["Tickets2"] = countTicket;
                    TempData["Thuis"] = cart.ThuisPloegNaam;
                    TempData["Bezoek"] = cart.BezoekersPloegNaam;
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