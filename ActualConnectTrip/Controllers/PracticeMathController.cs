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
        public ActionResult PracticeMath()
        {
            using (var context = new Entities())
            {
      
                var model = new PracticeMathViewModel
                {  
                    //mathQuestion = mObj.mathQuestion(game.level),
                   // mathAnswer = mObj.mathAnswer(mObj.mathQuestion(game.level))
                };
                return View();
            }
        }
    }
}