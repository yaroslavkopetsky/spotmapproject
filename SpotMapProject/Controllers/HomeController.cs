using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpotMapProject.Controllers;
using SpotMapProject.Models;



namespace SpotMapProject.Controllers
{
   //[InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private DataBaseController dbcon = new DataBaseController();

        public ActionResult Index() {
            int request_amount = dbcon.ShowEditRequests().Count();
            int spot_requests =  dbcon.GetSpotRequests().Count();
            List<int> spot_id = new List<int>();
            List<string> paths = new List<string>();
            List<AspNetSpot> spots = dbcon.GetRandomSpotsByAmount(6);
            if (spots != null)
            {
                foreach (AspNetSpot spot in spots)
                {
                    spot_id.Add(spot.Id);
                    paths.Add(dbcon.GetSpotFirstPhoto(Convert.ToString(spot.Id)));
                }
            }
            int sum = request_amount + spot_requests;
            ViewBag.SpotId = spot_id;
            ViewBag.Requests = sum;
            ViewBag.Paths = paths;
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Map()
        {
         

            return View();
        }

       



    }
}