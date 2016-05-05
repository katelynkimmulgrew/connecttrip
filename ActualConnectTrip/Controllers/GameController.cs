using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizLogic;
using Microsoft.AspNet.Identity;

namespace ActualConnectTrip.Controllers
{
    public class GameController : Controller
    {
        private Entities db = new Entities();
        static private Object lockObject = new object();
            

    // GET: Game
    public ActionResult Index()
        {
            return View();
        }

        public ActionResult Board(int id)
        {
            using(var db = new Entities())
            {
                Game board = db.getGameById(id);
                return View(board);
            }
            
        }

        [HttpPost]

        public ActionResult Board(int id, int col)
        {
            lock (lockObject)
            {
                using (var db = new Entities())
                {
                    Game board = db.getGameById(id);
                    Column currentCol = db.getCol(col, board);
                    Row currentRow = board.determinePlace(board.currentUser, col, db);
                    if(currentRow==null)
                    {
                        ViewBag.Message = "Cannot execute Move";
                        return RedirectToAction("Board", new { id = id });
                    }
                    if (board.determineWin(db, currentRow, currentCol))
                    {
                        return RedirectToAction("GameOver");
                    }
                    else
                    {
                        board.SwitchPlayers();
                    }
                    
                    return RedirectToAction("Board", new { id = id });
             }
            }
            
            

        }

        public ActionResult GameOver()
        {
            return View();
        }


        public ActionResult GameStats()
        {
            var currentUserId = User.Identity.GetUserId();
            //The below line throws an error
            // ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);   
            //get the dbContext???!!!!-> should a new one be created or DataLayer.Entities.cs is the dbContext to be used

            //get the user
            //assign the values to the view model object
            /* var model=new StatsViewModel()
             {
              overAllPercentageView= ...
             }*/
            return View();
        }
        
    }
}