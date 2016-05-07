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
        public ActionResult Board()
        {
            using(var db = new Entities())
            {
                
                var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                Game board = db.getGameById(currentPerson.CurrentGameId);
                bool? currentBool = board.currentUser;
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

        public ActionResult Board(int col)
        {
            lock (lockObject)
            {
                using (var db = new Entities())
                {
                    
                    var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                    
                    Game board = db.getGameById(currentPerson.CurrentGameId);
                    if(board==null)
                    {
                        return RedirectToAction("NoGame");
                    }
                    
                    bool? currentBool = board.currentUser;
                    var person1 = db.getPersonById(board.Player1Id);
                    var person2 = db.getPersonById(board.Player2Id);
                    if (board.finished == true)
                    {
                        ViewBag.Winner = db.getPersonById(board.winnerID).UserName + "won!";
                            return RedirectToAction("GameOver");
                    }
                    if (currentPerson!=person1&&currentPerson!=person2)
                    {
                        return RedirectToAction("stindex");
                    }
                    if (currentBool==currentPerson.assignedBool)
                    {
                        Column currentCol = db.getCol(col, board);
                        Row currentRow = board.determinePlace(board.currentUser, col, db);
                        if (currentRow == null)
                        {
                            ViewBag.Message = "Cannot execute Move";
                            return RedirectToAction("Board");
                        }
                        if (board.determineWin(db, currentRow, currentCol))
                        {
                            board.finished = true;
                            board.winnerID = currentPerson.Id;
                            ViewBag.Winner = db.getPersonById(board.winnerID).UserName + "won!";
                            return RedirectToAction("GameOver");
                        }
                        else
                        {
                            board.SwitchPlayers();
                        }
                        return RedirectToAction("Board");
                    }
                    else
                    {
                        ViewBag.Message = "It is not your turn";
                        return RedirectToAction("Board");
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
                var oldgame=(from c in enti.startGamePlayers
                                       where c.player1Id.Equals(infoUB.Id) 
                                       && c.isStarted.Equals(false)
                                       select c).FirstOrDefault();
                if(oldgame!=null)
                {
                    return RedirectToAction("waitingPage");
                }
            }

                using (Entities enti = new Entities())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                startInput.myid = infoUB.Id;

                var allWaiting = (from c in enti.startGamePlayers where c.isStarted.Equals(false) select c);

                var recommended = infoUB.findMatch(allWaiting, enti);

                if (recommended == null)
                {
                    startInput.Recommended = null;
                }
                else
                {
                    startInput.Recommended = recommended.ToList();
                }


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
                    newgame.finished = false;
                   
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
                    person1.CurrentGameId = newgame.Id;
                    person2.CurrentGameId = newgame.Id; 
                    enti.SaveChanges();
                    return RedirectToAction("Board");
                }
                //return View('game','GamePage');
            }

        }


        public ActionResult waitingPage()
        {
            var UserName = User.Identity.Name;
            using (Entities enti = new Entities())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                var oldgame = (from c in enti.startGamePlayers
                               where c.player1Id.Equals(infoUB.Id)
                               && c.isStarted.Equals(false)
                               select c).FirstOrDefault();
                if (oldgame != null)
                {
                    return View();
                }
                else { return RedirectToAction("stindex"); }
            }
            //return View();
        }

        [HttpPost]
        public ActionResult waitingPage(int cancel)
        {
            var UserName = User.Identity.Name;
            using (Entities enti = new Entities())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                var oldgame = (from c in enti.startGamePlayers
                               where c.player1Id.Equals(infoUB.Id)
                               && c.isStarted.Equals(false)
                               select c).FirstOrDefault();
                oldgame.isStarted = true;
                enti.SaveChanges();
            }
            return RedirectToAction("stindex");
        }


    }
}