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
    //check
    //check again
    public class GameController : Controller
    {
        private Entities2 db = new Entities2();
        static private Object lockObject = new object();
            

    // GET: Game
   
            public ActionResult Welcome()
        {
            return View();
        }
    public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }
        public ActionResult Image()
        {
            return View();
        }
        public ActionResult Board()


        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
               
            }
            using (var db = new Entities2())
            {

                var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                if (currentPerson.CurrentGameId == null)
                {
                    TempData["Message"] = "You do not have a game yet.  Please make one!";
                    return RedirectToAction("stindex");
                }
                Game board = db.getGameById((int)currentPerson.CurrentGameId);
                if (board == null)
                {
                    return View("NoGame");
                }
                if (board.finished == true)
                {
                    if(board.gameCancelled)
                    {
                        TempData["IsCancelled"] = "The game was cancelled!";
                    }
                    else
                    {
                        TempData["Winner"] = db.getPersonById(board.winnerID).UserName + " won!";
                    }

                    return View("GameOver");
                }
                if (currentPerson.answeredMathQuestion == false&&currentPerson.assignedBool==board.currentUser)
                {
                    //if (currentPerson.currentMathProblemID == null)
                    //{
                        mathProblemResult problemData = new mathProblemResult();
                        problemData.start = DateTime.Now;
                        db.mathProblemResults.Add(problemData);
                        db.SaveChanges();
                        currentPerson.currentMathProblemID = problemData.Id;
                        mathProblems problem = new mathProblems();
                        string question = problem.mathQuestion(board.level);
                        problemData.question = question;
                        string answer = problem.mathAnswer(question);
                        problemData.answer = answer;
                        
                        db.SaveChanges();
                        ViewBag.Question = problemData.question;
                    /*}
                    else
                    {
                        
                        int problemDataID = (int)currentPerson.currentMathProblemID;
                        mathProblemResult problemData = db.getmathProblemResultById(problemDataID);
                        problemData.start = DateTime.Now;
                        db.SaveChanges();
                        ViewBag.Question = problemData.question;
                    }*/
                    

                    
                }
                




                var person1 = db.getPersonById(board.Player1Id);
                var person2 = db.getPersonById(board.Player2Id);
                if (board.isFull(db))
                {
                    person1.isPlaying = false;
                    person2.isPlaying = false;
                    board.finished = true;
                    TempData["Winner"] = "No one won.  The board is full!";
                    db.SaveChanges();
                    return View("GameOver");
                }

                bool? currentBool = board.currentUser;
                if (currentPerson != person1 && currentPerson != person2)
                {
                    ViewBag.Error = "You do not have permission to view that game";
                    return View("Error");
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

        public ActionResult Board(int? col, string button, string answer)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            lock (lockObject)
            {
                using (var db = new Entities2())
                {
                    
                    var currentPerson = (from p in db.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                    
                    Game board = db.getGameById((int)currentPerson.CurrentGameId);
                   
                    if(currentPerson.answeredMathQuestion==false && answer!=null)
                    {
                        
                        currentPerson.answeredMathQuestion = true;
                        db.SaveChanges(); 
                        mathProblemResult problem = db.getmathProblemResultById((int)currentPerson.currentMathProblemID);
                        DateTime end = DateTime.Now;
                        TimeSpan diff = end - problem.start;
                        int differenceSeconds = diff.Seconds;
                        
                        TempData["Answer"] = problem.answer;
                        
                        bool isRight = problem.answer == answer;
                        problem.isRight = isRight;
                        TempData["isRight"] = isRight;
                        if (differenceSeconds > 120)
                        {
                            TempData["YourTurn"] = "You ran out of time and lost your turn";
                            
                            board.SwitchPlayers();
                            currentPerson.answeredMathQuestion = false;
                            currentPerson.DidNotAnswer++;
                            currentPerson.currentMathProblemID = null;

                            db.SaveChanges();
                            return View(board);
                        }
                        currentPerson.Answered++;
                        if (isRight == false)
                        {
                            
                             TempData["YourTurn"] = "You lost your turn";
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
                            currentPerson.overllAndsweredIncorrectly++;
                            board.SwitchPlayers();
                            currentPerson.answeredMathQuestion = false;
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
                        /*mathProblemResult problemData = new mathProblemResult();
                        db.mathProblemResults.Add(problemData);
                        db.SaveChanges();
                        currentPerson.currentMathProblemID = problemData.Id;
                        mathProblems problem2 = new mathProblems();
                        string question = problem2.mathQuestion(board.level);
                        problemData.question = question;
                        string answer2 = problem2.mathAnswer(question);
                        problemData.answer = answer;*/
                        currentPerson.currentMathProblemID = null;
                        
                        db.SaveChanges();
                        return View(board);
                    }
                    
                    
                    bool? currentBool = board.currentUser;
                    var person1 = db.getPersonById(board.Player1Id);
                    var person2 = db.getPersonById(board.Player2Id);

                    if (button == "cancel game")
                    {
                        board.finished = true;
                        board.gameCancelled = true;
                        person1.isPlaying = false;
                        person2.isPlaying = false;
                        person1.answeredMathQuestion = false;
                        person2.answeredMathQuestion = false;
                        TempData["IsCancelled"] = "This game was cancelled";
                        db.SaveChanges();
                        return View("GameOver");
                    }
                    
                    
                    if (currentBool==currentPerson.assignedBool)
                    {
                        //Column currentCol = db.getCol(col, board);
                        Row currentRow = board.determinePlace(board.currentUser, (int)col, db);
                        if (currentRow == null)
                        {
                            TempData["Message"] = "Cannot execute Move";
                            return View(board);
                        }
                        if (board.determineWin(db, currentRow))
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
                            TempData["Winner"] = db.getPersonById(board.winnerID).UserName + " won!";
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
                            person1.answeredMathQuestion = false;
                            person2.answeredMathQuestion = false;
                            db.SaveChanges();
                            return View("GameOver");
                        }
                        else
                        {
                            board.SwitchPlayers();
                            currentPerson.answeredMathQuestion = false;
                            db.SaveChanges();
                        }
                        return View(board);
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Cannot Execute when it is not your turn";
                        return View(board);
                    }
                    
             }
            }                      
        }

        

       

        

        public ActionResult GameStats()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            
            }
            using (var context = new Entities2())
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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            var UserName = User.Identity.Name;
            StartpageViewModel startInput = new StartpageViewModel();

            using (Entities2 enti = new Entities2())
            {


                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();

                if (infoUB.isPlaying == true)
                {
                    TempData["Message"] = "You are playing this game.  You cannot play a game until you complete or cancel this one.";
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

                using (Entities2 enti = new Entities2())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                startInput.myid = infoUB.Id;

                
                var allWaiting = (from c in enti.startGamePlayers where c.isStarted.Equals(false) select c);

                var recommended = infoUB.findMatch(allWaiting, enti);

                if (recommended == null)
                {
                    ViewBag.Recommended ="There are no recommendations";
                    startInput.isThereRecommended = false;
                }
                else
                {
                    startInput.Recommended = recommended.ToList();
                    startInput.isThereRecommended = true;
                    List<string> RCnames = new List<string>();
                    foreach (var plga in recommended)
                    {
                        var Names = (from c in enti.Persons
                                     where c.Id.Equals(plga.player1Id)
                                     select c.UserName).FirstOrDefault();
                        RCnames.Add(Names);
                    }
                    startInput.RCnames = RCnames;
                }
               
                    
                


                var watingGamer = (from c in enti.startGamePlayers
                                   where c.isStarted.Equals(false)
                                   && c.level.Equals(1)
                                   select c).ToList();

                startInput.L1rivals = watingGamer;

                var level1 = (from c in enti.startGamePlayers
                              where c.isStarted.Equals(false)
                              && c.level.Equals(1)
                              select c).FirstOrDefault();
                if(level1==null)
                {
                    startInput.isThereOtherGamesLevel1 = false;
                    
                }
                else
                {
                    startInput.isThereOtherGamesLevel1 = true;
                    List<string> Gamer1names = new List<string>();
                    foreach (var plga in watingGamer)
                    {
                        var Names = (from c in enti.Persons
                                     where c.Id.Equals(plga.player1Id)
                                     select c.UserName).FirstOrDefault();
                        Gamer1names.Add(Names);
                    }
                    startInput.L1names = Gamer1names;
                }

                var watingGamer2 = (from c in enti.startGamePlayers
                                    where c.isStarted.Equals(false)
                                    && c.level.Equals(2)
                                    select c).ToList();

               

                startInput.L2rivals = watingGamer2;
                var level2 = (from c in enti.startGamePlayers
                              where c.isStarted.Equals(false)
                              && c.level.Equals(2)
                              select c).FirstOrDefault();
                if (level2 == null)
                {
                    startInput.isThereOtherGamesLevel2 = false;
                    
                }
                else
                {
                    startInput.isThereOtherGamesLevel2 = true;
                    List<string> Gamer2names = new List<string>();
                    
                        foreach (var plga in watingGamer2)
                        {
                        var Names = (from c in enti.Persons
                                     where c.Id.Equals(plga.player1Id)
                                     select c.UserName).FirstOrDefault();
                        Gamer2names.Add(Names);
                    }
                    startInput.L2names = Gamer2names;
                }
                var watingGamer3 = (from c in enti.startGamePlayers
                                    where c.isStarted.Equals(false)
                                    && c.level.Equals(3)
                                    select c).ToList();

                startInput.L3rivals = watingGamer3;

                var level3 = (from c in enti.startGamePlayers
                              where c.isStarted.Equals(false)
                              && c.level.Equals(3)
                              select c).FirstOrDefault();
                if (level3 == null)
                {
                    startInput.isThereOtherGamesLevel3 = false;
                    
                }
                else
                {
                    startInput.isThereOtherGamesLevel3 = true;
                    List<string> Gamer3names = new List<string>();
                    foreach (var plga in watingGamer3)
                    {
                        var Names = (from c in enti.Persons
                                     where c.Id.Equals(plga.player1Id)
                                     select c.UserName).FirstOrDefault();
                        Gamer3names.Add(Names);
                    }
                    startInput.L3names = Gamer3names;

                }

            }
            
            return View(startInput);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult stindex(StartpageViewModel startdata)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            if (startdata.request == true)
            {
                lock (lockObject)
                {
                    startGamePlayer newstart = new startGamePlayer();
                    newstart.player1Id = startdata.myid;
                    newstart.level = startdata.gamelevel;
                    newstart.isStarted = false;

                    using (Entities2 enti = new Entities2())
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
                    using (Entities2 enti = new Entities2())
                    {
                        Game newgame = new Game();
                        enti.Games.Add(newgame);
                        enti.SaveChanges();
                        newgame.maxCols = 7;
                        newgame.maxRows = 6;

                        for (int i = 1; i <= newgame.maxCols; i++)
                        {
                            //Column column = new Column();
                           // enti.SaveChanges();
                            //enti.Columns.Add(column);
                           // newgame.theColumns.Add(column);
                            //column.ColumnNumber = i;
                            for (int j = 1; j <= newgame.maxRows; j++)
                            {
                                Row row = new Row { RowNumber = j, columnNumber = i, gameID = newgame.Id, Value = null };
                                enti.Rows.Add(row);
                                //column.theRows.Add(row);
                                enti.SaveChanges();
                                

                            }
                            
                        }
                            
                            enti.SaveChanges();

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
                            person1.assignedBool = false;

                            var person2 = (from c in enti.Persons
                                           where c.Id.Equals(newgame.Player2Id)
                                           select c).FirstOrDefault();
                            person2.assignedBool = true;                       // so when a player accept the game, he will be player2 
                                                                                // and set to false
                            person1.CurrentGameId = newgame.Id;
                            person2.CurrentGameId = newgame.Id;
                            person1.isPlaying = true;
                            person2.isPlaying = true;
                            newgame.Player1Id = person1.Id;
                            newgame.Player2Id = person2.Id;
                        newgame.gameCancelled = false;
                        
                            enti.SaveChanges();
                            ViewBag.Message = newgame.Id.GetType().ToString() + newgame.Id.ToString();
                            return RedirectToAction("Board");
                        }
                    }

                    //return View('game','GamePage');
                }

            }
        


        public ActionResult Forum()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            using (Entities2 enti = new Entities2())
            {

                
                forumViewModel tempone = new forumViewModel();

                tempone.model1 = enti.Questions.Include("answers2").ToList();
                tempone.model2 = null;
                tempone.model3 = enti.theAnswers.ToList();
                return View(tempone);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forum(forumViewModel ques, string keyword) /*[Bind(Include = "Id,title,description")] Ques ques*/
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            using (Entities2 enti = new Entities2())
            {
                if (ques.model2 != null && ques.model2.description != null && ques.model2.title!=null)
                {


                    enti.Questions.Add(ques.model2);
                    enti.SaveChanges();


                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = enti.Questions.Include("answers2").ToList();
                    tempone.model2 = null;
                    return View(tempone);

                }
                else if (ques.model4!=null && ques.model4.content != null)
                {
                    enti.Getquesforid(ques.model5).answers2.Add(ques.model4);
                    enti.SaveChanges();
                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = enti.Questions.Include("answers2").ToList();
                    //tempone.model2 = null;
                    ModelState.Clear();
                    return View(tempone);
                }
                else if (keyword != null)
                {


                    //var infoUB = (from c in db.Questions
                    //              where c.title.ToString().Contains(keyword)
                    //              select c);
                    var infoUB = (from c in enti.Questions.Include("answers2")
                                  where c.title.ToString().Contains(keyword)
                                  select c);
                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = infoUB.ToList();
                    tempone.model2 = null;
                    return View(tempone);

                }

                else
                {
                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = enti.Questions.Include("answers2").ToList();
                    tempone.model2 = null;
                    return View(tempone);
                }
            }
        }

        public ActionResult waitingPage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            var UserName = User.Identity.Name;
            using (Entities2 enti = new Entities2())
            {
                var infoUB = (from c in enti.Persons
                              where c.UserName.Equals(UserName)
                              select c).FirstOrDefault();
                if (infoUB.isPlaying == true)
                {
                    TempData["Message"] = "You are playing this game.  You cannot play a game until you complete or cancel this one.";
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
        public ActionResult waitingPage(StartpageViewModel startdata)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            var UserName = User.Identity.Name;
            using (Entities2 enti = new Entities2())
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


        public static string mathQuestion_external;
        public static string mathAnswer_external;
        public static bool PracticeMathFlag = false;
        [HttpPost]
        public ActionResult PracticeMath(PracticeMathViewModel inputdata)
        {
            if (inputdata.isTryAgainBlockVisable)
            {
                if (inputdata.isTryAgain)
                {
                    var model = new PracticeMathViewModel()
                    {
                        mathQuestion = mathQuestion_external,
                        isSelectLevelBlockVisable = false,
                        isAnswerBlockVisable = true
                    };
                    return View(model);
                }
                else
                {
                    var model = new PracticeMathViewModel()
                    {
                        mathQuestion = mathQuestion_external,
                        mathAnswer = mathAnswer_external,
                        isNextQuesitonBlockVisable = true,
                        isShowAnswer = true
                    };
                    return View(model);
                }
            }

            if (PracticeMathFlag == true)
            {
                var mobj = new BizLogic.mathProblems();
                var model = new PracticeMathViewModel()
                {
                    mathQuestion = mobj.mathQuestion(inputdata.levelchosen),
                    isSelectLevelBlockVisable = false,
                    isAnswerBlockVisable = true
                };
                mathQuestion_external = model.mathQuestion;
                PracticeMathFlag = false;
                return View(model);
            }
            else
            {
                var model = new PracticeMathViewModel()
                {
                    isSelectLevelBlockVisable = false,
                    isAnswerBlockVisable = false
                };
                //var answer = inputdata.mathAnswer;
                var mobj = new BizLogic.mathProblems();
                var realAnswer = mobj.mathAnswer(inputdata.mathQuestion);
                mathAnswer_external = realAnswer;
                if (!realAnswer.Equals(inputdata.userAnswer))
                {
                    ViewBag.message = "Your Answer is Wrong";
                    model.isTryAgainBlockVisable = true;
                    return View(model);
                }
                else
                {
                    ViewBag.message = "Your Answer is Right";
                    model.isNextQuesitonBlockVisable = true;
                    return View(model);
                }
            }        
        }

        public ActionResult PracticeMath()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not authenticated to see this page.";
                return View("../Home/PermissionDenied");
            }
            using (var context = new Entities2())
            {
                var model = new PracticeMathViewModel()
                {
                    isSelectLevelBlockVisable = true,
                    isAnswerBlockVisable = false,
                    isShowAnswer = false
                };
                PracticeMathFlag = model.isSelectLevelBlockVisable;
                return View(model);
            }
        }
        
        
       /* public PartialViewResult EachTurnMathQuestion(int level)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ErrorMessage = "You are not authenticated to see this page.";
               return PartialView("PermissionDenied","Home");
            }
            using (var context = new Entities2())
            {
                var mathObj = new mathProblems();
                var mathViewModel = new PracticeMathViewModel()
                {
                    mathQuestion = mathObj.mathQuestion(level)
                };
                return PartialView("EachTurnMathQuestion");
            }
        }*/
    }
}