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
    public class StatsController : Controller
    {
        private Entities db = new Entities();
        // GET: Stats
        public ActionResult Index()
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