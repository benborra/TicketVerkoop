using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Payment(List<CartViewModel> cart)
        {
            string userName = User.Identity.Name;

            try
            {

            }
            catch (Exception ex)
            {

            }

            return View();
        }
    }
}