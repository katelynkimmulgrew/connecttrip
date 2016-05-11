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
            using (var db = new Entities())
            {

                var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                Game board = db.getGameById(currentPerson.CurrentGameId);
                if (board == null)
                {
                    return RedirectToAction("NoGame");
                }
                if (board.finished == true)
                {

                    ViewBag.Winner = db.getPersonById(board.winnerID).UserName + "won!";

                    return RedirectToAction("GameOver");
                }
                if (currentPerson.answeredMathQuestion == false)
                {
                    if(currentPerson.currentMathProblemID == null) {
                        mathProblemResult problemData = new mathProblemResult();
                        currentPerson.currentMathProblemID = problemData.Id;
                        mathProblems problem = new mathProblems();
                        string question = problem.mathQuestion(board.level);
                        problemData.question = question;
                        string answer = problem.mathAnswer(question);
                        problemData.answer = answer;
                        db.mathProblemResults.Add(problemData);
                        db.SaveChanges();
                        ViewBag.Question = question;
                    }

                }
                else {
                    ViewBag.Question = db.getmathProblemResultById((int)currentPerson.currentMathProblemID).question;

                }




                var person1 = db.getPersonById(board.Player1Id);
                var person2 = db.getPersonById(board.Player2Id);
                if (board.isFull(db))
                {
                    person1.isPlaying = false;
                    person2.isPlaying = false;
                    board.finished = true;
                    ViewBag.Winner = "No one won.  The board is full!";
                    db.SaveChanges();
                    return RedirectToAction("GameOver");
                }

                bool? currentBool = board.currentUser;
                if (currentPerson != person1 && currentPerson != person2)
                {
                    ViewBag.Error = "You do not have permission to view that game";
                    return RedirectToAction("Error");
                }
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

        public ActionResult Board(int col, string button, string answer)
        {
            
            lock (lockObject)
            {
                using (var db = new Entities())
                {
                    
                    var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                    
                    Game board = db.getGameById(currentPerson.CurrentGameId);
                   
                    if(currentPerson.answeredMathQuestion==false && answer!=null)
                    {
                        currentPerson.answeredMathQuestion = true;
                        mathProblemResult problem = db.getmathProblemResultById((int)currentPerson.currentMathProblemID);
                        ViewBag.Answer = problem.answer;
                        ViewBag.isRight = problem.answer == answer;
                        bool isRight = problem.answer == answer;
                        if (isRight == false)
                        {
                            ViewBag.YourTurn = "You lost your turn";
                            if (board.level == 1)
                            {
                                currentPerson.levelOneAnsweredIncorrectly++;

                            }
                            else if (board.level == 2)
                            {
                                currentPerson.levelTwoAnsweredIncorrectly++;
                            }
                            else
                            {
                                currentPerson.levelThreeAnsweredIncorrectly++;
                            }
                            currentPerson.overllAnsweredIncorrectly++;
                            db.SaveChanges();

                        }
                        else
                        {
                            if (board.level == 1)
                            {
                                currentPerson.levelOneAnsweredCorrectly++;

                            }
                            else if (board.level == 2)
                            {
                                currentPerson.levelTwoAnsweredCorrectly++;
                            }
                            else
                            {
                                currentPerson.levelThreeAnsweredCorrectly++;
                            }
                            currentPerson.overallAnsweredCorrectly++;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Board");
                    }

                    
                    bool? currentBool = board.currentUser;
                    var person1 = db.getPersonById(board.Player1Id);
                    var person2 = db.getPersonById(board.Player2Id);

                    if (button == "cancel")
                    {
                        board.finished = true;
                        person1.isPlaying = false;
                        person2.isPlaying = false;
                        ViewBag.IsCancelled = "This game was cancelled";
                        db.SaveChanges();
                        return RedirectToAction("GameOver");
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
                            currentPerson.isPlaying = false;
                            Person otherPerson;
                            if(currentPerson==person1)
                            {
                                otherPerson = person2;
                            }
                            else
                            {
                                otherPerson = person1;
                            }
                            otherPerson.isPlaying = false;
                            ViewBag.Winner = db.getPersonById(board.winnerID).UserName + "won!";
                            if (board.level == 1)
                            {
                                currentPerson.LevelOneWins++;
                                otherPerson.LevelOneLose++;

                            }
                            else if (board.level == 2)
                            {
                                currentPerson.LevelTwoWins++;
                                otherPerson.LevelTwoLose++;
                            }
                            else
                            {
                                currentPerson.LevelThreeWins++;
                                otherPerson.LevelThreeLose++;
                            }
                            db.SaveChanges();
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
                        ViewBag.Message = "Invalid Cannot Execute when it is not your turn";
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
                    //didNotAnswerView = statsAndRecommendationLogic.didNotAnwserPercentage(currentPerson),
                    totalNumberOfGames = statsAndRecommendationLogic.numGames(currentPerson),
                    totalNumberOfWins = statsAndRecommendationLogic.numWins(currentPerson),
                    totalNumberOfLose = statsAndRecommendationLogic.numLose(currentPerson),
                    GameComplimentView = statsAndRecommendationLogic.GameCompliment(currentPerson),
                    levelOneMathPercentage=statsAndRecommendationLogic.levelOneMathCorrectPerecentage(currentPerson),
                    levelTwoMathPercentage=statsAndRecommendationLogic.levelTwoMathCorrectPerecentage(currentPerson),
                    levelThreeMathPercentage=statsAndRecommendationLogic.levelThreeMathCorrectPerecentage(currentPerson),
                    overAllCorrectAnswersPercentage=statsAndRecommendationLogic.overallMathPercentage(currentPerson),
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
                if (infoUB.isPlaying == true)
                {
                    ViewBag.Message = "You are playing this game.  You cannot play a game until you complete or cancel this one.";
                    return RedirectToAction("Board");
                }

                var oldgame =(from c in enti.startGamePlayers
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
                    ViewBag.Recommended = ": There are no recommendations";
                }
               
                    startInput.Recommended = recommended.ToList();
                


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
                lock(lockObject)
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
                }
                
                //return View();
            }
            else
            {
                lock (lockObject)
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
                        newgame.currentUser = true;

                        var person1 = (from c in enti.Persons
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
                        person1.isPlaying = true;
                        person2.isPlaying = true;
                        newgame.Player1Id = person1.Id;
                        newgame.Player2Id = person2.Id;
                        enti.SaveChanges();
                        return RedirectToAction("Board");
                    }
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
                if (infoUB.isPlaying == true)
                {
                    ViewBag.Message = "You are playing this game.  You cannot play a game until you complete or cancel this one.";
                    return RedirectToAction("Board");
                }
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

        [HttpPost]
        public ActionResult PracticeMath(PracticeMathViewModel inputdata)
        {

            var level = inputdata.levelchosen;
            using (var context = new Entities())
            {
                var mobj = new BizLogic.mathProblems();
                var model1 = new PracticeMathViewModel()
                {
                    mathQuestion = mobj.mathQuestion(level)
                };
                return View();
            }
        }
        public ActionResult PracticeMath()
        {
            return View();
        }

        public PartialViewResult EachTurnMathQuestion(int level)
        {
            using (var context = new Entities())
            {
                var mathObj = new mathProblems();
                var mathViewModel = new PracticeMathViewModel()
                {
                    mathQuestion = mathObj.mathQuestion(level)
                };
                return PartialView("EachTurnMathQuestion");
            }
        }
    }
}