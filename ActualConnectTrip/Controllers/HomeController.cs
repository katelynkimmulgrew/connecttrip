using ActualConnectTrip.Models;
using BizLogic;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActualConnectTrip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Userlist()
        {
            ViewBag.Message = "The User List";
            ViewBag.Nouser = "Not any user yet.";

            using (var db = new Entities2())
            {
                List<Person> tempone = (db.Persons.Include("CatchPhrase").OrderBy(a=>a.overallPercentage())).ToList();
                return View(tempone);
            }

                
        }

    }
}