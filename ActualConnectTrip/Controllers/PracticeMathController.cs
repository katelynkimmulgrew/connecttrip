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
           
            var currentUserId = User.Identity.GetUserId();
            //how to get current user though userID?

            BizLogic.mathProblems mObj = new BizLogic.mathProblems();
        /*    var model = new PracticeMathViewModel
            {
                //mathQuestion = mObj.mathQuestion(User.level),
                //mathAnswer = mObj.mathAnswer(mObj.mathQuestion(User.level))
            };*/

            return View();
        }
    }
}