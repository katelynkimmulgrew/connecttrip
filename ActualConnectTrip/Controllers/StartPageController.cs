using ActualConnectTrip.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActualConnectTrip.Controllers
{
    public class StartPageController : Controller
    {
        // GET: StartPage
        public ActionResult stindex()
        {
            var userId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult stindex(StartpageViewModel startdata)
        {
            return View();
        }
    }
}