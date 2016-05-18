using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Ticket.Model;
using Ticket.Service;

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