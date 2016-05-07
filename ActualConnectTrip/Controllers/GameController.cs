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


        public ActionResult stindex()
        {
            var UserName = User.Identity.Name;
            StartpageViewModel startInput = new StartpageViewModel();

            using (Entities enti = new Entities())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                startInput.myid = infoUB.Id;

                var watingGamer = (from c in enti.startGamePlayers
                                   where c.isStarted.Equals(false)
                                   && c.level.Equals(1)
                                   select c).Take(3).ToList();

                startInput.L1rivals = watingGamer;

                var watingGamer2 = (from c in enti.startGamePlayers
                                    where c.isStarted.Equals(false)
                                    && c.level.Equals(2)
                                    select c).Take(3).ToList();

                startInput.L2rivals = watingGamer2;

                var watingGamer3 = (from c in enti.startGamePlayers
                                    where c.isStarted.Equals(false)
                                    && c.level.Equals(3)
                                    select c).Take(3).ToList();

                startInput.L3rivals = watingGamer3;
            }
            return View(startInput);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult stindex(StartpageViewModel startdata)
        {
            if (startdata.request == true)
            {
                startGamePlayer newstart = new startGamePlayer();
                newstart.player1Id = startdata.myid;
                newstart.level = startdata.gamelevel;
                newstart.isStarted = false;
                using (Entities enti = new Entities())
                {
                    enti.startGamePlayers.Add(newstart);
                    enti.SaveChanges();

                }
                return RedirectToAction("waitingPage");
                //return View();
            }
            else
            {
                using (Entities enti = new Entities())
                {
                    Game newgame = ConnectTripLogic.setBoard(enti);
                    newgame.Player1Id = startdata.oppoid ?? default(int);
                    newgame.Player2Id = startdata.myid;
                    newgame.level = startdata.gamelevel;
                   
                    int ID = newgame.Id;
                    var removeStart = (from c in enti.startGamePlayers
                                       where c.player1Id.Equals(newgame.Player1Id)
                                       && c.isStarted.Equals(false)
                                       select c).FirstOrDefault();
                    removeStart.isStarted = true;
                    newgame.currentUser =true;

                    var person1= (from c in enti.Persons
                                  where c.Id.Equals(newgame.Player1Id)
                                  select c).FirstOrDefault();
                    person1.assignedBool = true;

                    var person2 = (from c in enti.Persons
                                   where c.Id.Equals(newgame.Player2Id)
                                   select c).FirstOrDefault();
                    person2.assignedBool = false;                       // so when a player accept the game, he will be player2 
                                                                        // and set to false
                    enti.SaveChanges();
                    return RedirectToAction("Board",new { id=ID});
                }
                //return View('game','GamePage');
            }

        }


        public ActionResult waitingPage()
        {
            return View();
        }


        }
}