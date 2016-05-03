using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ticket.Service;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ProfileController : Controller
    {
        UserService userService;

        public new ActionResult View()
        {
            userService = new UserService();

            var user = userService.Get(User.Identity.Name);

            return View(user);
        }
    }
}