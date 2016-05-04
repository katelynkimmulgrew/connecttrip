using ActualConnectTrip.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace ActualConnectTrip.Controllers
{
    public class StartPageController : Controller
    {
        // GET: StartPage
        public ActionResult stindex()
        {
            //var userId = User.Identity.GetUserId();
            var UserName = User.Identity.Name;
            StartpageViewModel startInput = new StartpageViewModel();
            //startInput.myid = ;
            using (Entities enti = new Entities())
            {
                var infoUB = (from c in enti.Users
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                startInput.myid = infoUB.Id;

                var watingGamer = (from c in enti.startGamePlayers
                                   where c.isStarted.Equals(false)
                                   select c).Take(3).ToList() ;

                startInput.rivals = watingGamer;
            }
                return View(startInput);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult stindex(StartpageViewModel startdata)
        {
            if(startdata.request==true)
            {
                startGamePlayer newstart = new startGamePlayer();
                newstart.player1Id = startdata.myid;
                using (Entities enti = new Entities())
                {
                    enti.startGamePlayers.Add(newstart);
                    enti.SaveChanges();

                }
                //return View('wating','StartPage');
                return View();
            }
            else
            {
                using (Entities enti = new Entities())
                {
                    Game newgame = new Game();
                    newgame.Player1Id = startdata.oppoid ?? default(int);
                    newgame.Player2Id = startdata.myid;
                    enti.Games.Add(newgame);

                    var removeStart = (from c in enti.startGamePlayers
                                       where c.player1Id.Equals(newgame.Player1Id)
                                       select c).FirstOrDefault();
                    removeStart.isStarted = true;
                    enti.SaveChanges();
                }
                //return View('game','GamePage');
                return View();
            }
            
        }
    }
}