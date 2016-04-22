using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ActualConnectTrip.Models;

namespace ActualConnectTrip.Controllers
{
    public class StatsController : Controller
    {
        // GET: Stats
        public ActionResult Index()
        {
            //get the dbContext???!!!!-> should a new one be created or DataLayer.Entities.cs is the dbContext to be used
            string currentUserId = User.Identity.GetUserId();
           //get the user
           //assign the values to the view model object
          /* var model=new StatsViewModel()
           {
            overAllPercentageView= 
           }*/
           
            return View();
        }
    }
}