using BeerPongTracker.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerPongTracker.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ScreenViewModel()
            {
                Screen1ViewModel= new Screen1ViewModel()
            };

            return View(model);
        }
    }
}
