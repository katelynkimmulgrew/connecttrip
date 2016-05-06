using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ActualConnectTrip.Models;
using DataLayer;

namespace ActualConnectTrip.Controllers
{
    public class PracticeMathController : Controller
    {
        
        private Entities db = new Entities();
        // GET: PracticeMath
        public ActionResult Index()
        {
            using (var context = new Entities())
            {
                var currentPerson = (from p in context.Persons where p.UserName == User.Identity.Name select p).FirstOrDefault();
                BizLogic.mathProblems mObj = new BizLogic.mathProblems();
                var model = new PracticeMathViewModel
                {
                     mathQuestion = mObj.mathQuestion(currentPerson.level),
                     mathAnswer = mObj.mathAnswer(mObj.mathQuestion(currentPerson.level))
                };

                return View();
            }
        }
    }
}