using ActualConnectTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using BizLogic;

namespace ActualConnectTrip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Game");
            }
        }

        public ActionResult Rules()
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
            ViewBag.Message = "Contact Us!";

            return View();
        }

        public ActionResult MyProfile(int id)
        {
            using (var db = new Entities2())
            {
                Person p = db.getPersonById(id);
                if (p == null)
                {
                    return View("Error");
                }

                ProfilePageViewModel profile = new ProfilePageViewModel();
                profile.userName = p.UserName;
                profile.catchphraseView = p.CatchPhrase;
                profile.levelOnePercentageView = p.levelOnePercentage();
                profile.levelTwoPercentageView = p.levelTwoPercentage();
                profile.levelThreePercentageView = p.levelThreePercentage();
                profile.totalNumberOfWins = p.numWins();
                profile.totalNumberOfLose = p.numLose();
                profile.overAllPercentageView = p.overallPercentage();

                return View(profile);
            }
        }



        public ActionResult Userlist()
        {
            ViewBag.Message = "The User List";
            ViewBag.Nouser = "Not any user yet.";

            using (var db = new Entities2())
            {
                List<Person> tempone = (db.Persons.Include("CatchPhrase").OrderBy(a => a.overallPercentage())).ToList();
                return View(tempone);
            }

        }


    }
}