﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketVerkoopVoetbal.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Home
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}