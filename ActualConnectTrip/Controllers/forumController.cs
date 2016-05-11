using DataLayer;
using ActualConnectTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ActualConnectTrip.Controllers
{
    public class ReadController : Controller
    {
        //private ForumContext db = new ForumContext();
        // GET: Read
        public ActionResult Show()
        {
            using (ForumContext db = new ForumContext())
            {
                //int an = 1;
                //foreach (Ques questt in db.Questions)
                //{
                //    if (questt.answers != null)
                //        questt.answers.Add(new Answer() { content = an.ToString()});
                //    an = an + 1;
                //}
                //Ques questt = db.Getquesforid(1);
                //questt.answers.Add(new Answer() { content = "another more answer" });



                db.SaveChanges();
                forumViewModel tempone = new forumViewModel();

                tempone.model1 = db.Questions.Include("answers").ToList();
                tempone.model2 = null;
                tempone.model3 = db.Answers.ToList();
                return View(tempone);
            }
                
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Show(forumViewModel ques, string keyword) /*[Bind(Include = "Id,title,description")] Ques ques*/
        {
            using (ForumContext db = new ForumContext())
            {
                if (ques.model2 != null)
                {
                    
                    
                        db.Questions.Add(ques.model2);
                        db.SaveChanges();


                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = db.Questions.Include("answers").ToList();
                    tempone.model2 = null;
                    return View(tempone);

                }
                else if(ques.model4 != null)
                {
                    db.Getquesforid(ques.model5).answers.Add(ques.model4);
                    db.SaveChanges();
                    forumViewModel tempone = new forumViewModel();
                    tempone.model1 = db.Questions.Include("answers").ToList();
                    //tempone.model2 = null;
                    ModelState.Clear();
                    return View(tempone);
                }
                else if (keyword != null)
                {


                    //var infoUB = (from c in db.Questions
                    //              where c.title.ToString().Contains(keyword)
                    //              select c);
                    var infoUB = (from c in db.Questions.Include("answers")
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
                    tempone.model1 = db.Questions.Include("answers").ToList();
                    tempone.model2 = null;
                    return View(tempone);
                }            
            }
        }




    }
}