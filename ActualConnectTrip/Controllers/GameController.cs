using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizLogic;
using Microsoft.AspNet.Identity;
using ActualConnectTrip.Models;

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
                bool? currentBool = board.currentUser;
                var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                if (currentBool == currentPerson.assignedBool)
                {
                    ViewBag.Turn = "It is your turn";
                }
                else
                {
                    ViewBag.Turn = "It is not your turn";
                }
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
                    bool? currentBool = board.currentUser;
                    var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                    
                    if (currentBool==currentPerson.assignedBool)
                    {
                        Column currentCol = db.getCol(col, board);
                        Row currentRow = board.determinePlace(board.currentUser, col, db);
                        if (currentRow == null)
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
                    else
                    {
                        ViewBag.Message = "It is not your turn";
                        return RedirectToAction("Board", new { id = id });
                    }
                    
             }
            }                      
        }

        

        public ActionResult GameOver()
        {
            return View();
        }

        

        public ActionResult GameStats()
        {
            using (var context = new Entities())
            {
                
                var currentPerson = (from p in context.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                var model = new StatsViewModel
                {
                    overAllPercentageView = statsAndRecommendationLogic.overallPercentage(currentPerson),
                    levelOnePercentageView = statsAndRecommendationLogic.levelOnePercentage(currentPerson),
                    levelTwoPercentageView = statsAndRecommendationLogic.levelThreePercentage(currentPerson),
                    levelThreePercentageView = statsAndRecommendationLogic.levelThreePercentage(currentPerson),
                    didNotAnswerView = statsAndRecommendationLogic.didNotAnwserPercentage(currentPerson),
                    totalNumberOfGames = statsAndRecommendationLogic.numGames(currentPerson),
                    totalNumberOfWins = statsAndRecommendationLogic.numWins(currentPerson),
                    totalNumberOfLose = statsAndRecommendationLogic.numLose(currentPerson),
                    GameComplimentView = statsAndRecommendationLogic.GameCompliment(currentPerson),
                    MathComplimentView = statsAndRecommendationLogic.MathCompliment(currentPerson)
                };
                return View(model);
            }             
        }        
    }
}