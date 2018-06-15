using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpotMapProject.Models;
using DataModel;
using SpotMapProject.Controllers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;

namespace SpotMapProject.Controllers
{
    [Authorize]
    public class SpotController : Controller
    {
        private Controllers.DataBaseController dbcon = new DataBaseController();
        private DetailsViewModel.MyViewModel det_view_model = new DetailsViewModel.MyViewModel();
        private ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        // GET: Spot
        public ActionResult Index()
        {
            return View();
        }

        //Role=("Moderator","Admin")
        [Authorize]
        [HttpGet]
        public ActionResult AddSpot(string lon,string lat)
        {
            
            AspNetSpot model = new AspNetSpot();
            
              
            return View(model);

        }
        //Role=("Moderator","Admin")
        [Authorize]
        [HttpPost]
        public ActionResult AddSpot(AspNetSpot model, HttpPostedFileBase[] files)
        {
            if (files[0] != null)
            {
                foreach (HttpPostedFileBase file in files)
                {

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/SpotImages"),
                                                   Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                        dbcon.AddPhotoPath(Convert.ToString(dbcon.GetLastSpotID() + 1), file.FileName);
                        
                    }


                }
            }
            if (files[0] == null)
                {               
                    dbcon.AddPhotoPath(Convert.ToString(dbcon.GetLastSpotID() + 1), "default.png");
                }

                dbcon.AddSpot(model);
                dbcon.AllowEdit(dbcon.GetLastSpotID().ToString());
                dbcon.AddNewAction("Added new spot with ID = " + dbcon.GetLastSpotID());


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult RequestSpot(AspNetSpot model , HttpPostedFileBase[] files)
        {
            if (files[0] != null)
            {
                foreach (HttpPostedFileBase file in files)
                {


                    string path = Path.Combine(Server.MapPath("~/Content/SpotImages"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    dbcon.AddPhotoPath(Convert.ToString(dbcon.GetLastSpotID() + 1), file.FileName);

                }
            }
            if (files[0] == null)
            {
                dbcon.AddPhotoPath(Convert.ToString(dbcon.GetLastSpotID() + 1), "default.png");
            }

                    dbcon.AddSpotRequest(model);
                dbcon.AllowEdit(dbcon.GetLastSpotID().ToString());
            dbcon.AddNewAction("Requested new spot with ID = " + dbcon.GetLastSpotID());
            return View();
        }


        public JsonResult GetSpotsJSON()
        {

            List<AspNetSpot> spots = new List<AspNetSpot>();
            spots = dbcon.GetAllVisibleSpots();
            var jsonResult = Json(spots, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public ActionResult Details(string id) //View Spot Details By Id
        {

            AspNetSpot spot = new AspNetSpot();

            spot = dbcon.GetSpotDataByID(id);
            if (spot == null || spot.author != user.UserName && spot.@public == false)
            {
               
                
                    return View("Error");
                
            }
            det_view_model.SpotData = spot;
            det_view_model.SpotComments = dbcon.GetSpotCommentsByID(id);
            det_view_model.Usernames = dbcon.GetCommentUsernameByID(id);
            det_view_model.Date = dbcon.GetSpotCommentDateByID(id);
            det_view_model.Rating = Convert.ToString(dbcon.GetSpotRatingValueByID(id));
            det_view_model.editor = dbcon.CheckUserEditor(id);
            det_view_model.photos= dbcon.GetAllSpotPhotosByID(id);
            det_view_model.favorite = dbcon.CheckInFavoriteByID(id);
            return View(det_view_model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(string text, string id)
        {
           
            dbcon.AddComment(id, text);
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize]
        public ActionResult RateSpot(string id, string value)
        {

            dbcon.RateSpot(id, value);

            return RedirectToAction("Details", new { id = id }); ;
        }

        [Authorize(Roles ="admin,moder")]
        [HttpGet]
        public ActionResult ShowSpotRequests()
        {
           
            return View(dbcon.GetSpotRequests());
        }

       public ActionResult AcceptRequest(string id)
        {
            dbcon.AcceptSpotRequest(id);
            dbcon.AddNewAction("Accepted spot with ID = " + id);
            return RedirectToAction("ShowSpotRequests");
        }
        [HttpGet]   
        public ActionResult UpdateSpot(string id)
        {
            AspNetSpot spot = new AspNetSpot();

            spot = dbcon.GetSpotDataByID(id);
            ViewBag.spotid = id;
            ViewBag.photos = dbcon.GetAllSpotPhotosByID(id);

            if (spot == null || spot.author != user.UserName && spot.@public == false)
            {
                return View("Error");
            }
           

            return View(spot);
        }

        [HttpPost]
        public ActionResult UpdateSpot(AspNetSpot model, HttpPostedFileBase[] files)
        {
            if (files[0] != null)
            {
                foreach (HttpPostedFileBase file in files)
                {

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/SpotImages"),
                                                   Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                        dbcon.AddPhotoPath(Convert.ToString(model.Id), file.FileName);

                    }


                }
            }
            

            dbcon.UpdateSpot(model);
            dbcon.AddNewAction("Updated spot with ID = " + model.Id);
            return RedirectToAction("Map","Home");
        }

       [Authorize(Roles =("admin,moder"))]
        public ActionResult DeleteSpot(string id)
        {

            dbcon.DeleteSpot(id);
            dbcon.AddNewAction("Deleted spot with ID = " + id);
            return RedirectToAction("ShowSpots","Roles");
        }

        public ActionResult AddSpotToFavorites(string id)
        {
            dbcon.AddToFavoriteSpot(id);


            return RedirectToAction("Details", new { id = id });
        }




        public ActionResult ShowFavoriteSpots(string id)
        {
            // Show FavSpots


            return View(dbcon.GetFavoriteSpotList());
        }

        [HttpGet]
        public ActionResult ShowSpot(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public ActionResult UpdateImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateImage(string spot_id,string index, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/SpotImages"),
                                           Path.GetFileName(file.FileName));
                file.SaveAs(path);
                ViewBag.Message = "File uploaded successfully";
                dbcon.AddPhotoPath(Convert.ToString(dbcon.GetLastSpotID() + 1), file.FileName);
                List<AspNetSpotPhoto> models = dbcon.GetAllSpotPhotosToListById(spot_id);
                dbcon.UpdatePhoto(models[Convert.ToInt32(index)], file.FileName);

            }



            return RedirectToAction("Details", new { id = spot_id });
        }
    }
}