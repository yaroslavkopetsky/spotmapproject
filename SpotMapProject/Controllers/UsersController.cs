using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpotMapProject.Controllers;
using SpotMapProject.Models;
namespace SpotMapProject.Controllers
{
    
    public class UsersController : Controller
    {
        private DataBaseController dbcon = new DataBaseController();
        // GET: Users
        [Authorize(Roles = "admin,moder")]
        [HttpGet]
        public ActionResult ShowUsers()
        {


            
            return View(dbcon.GetAllUsers());
        }


        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {

            dbcon.DeleteUserByID(id);

            return View();
        }
        [Authorize(Roles = "admin,moder")]
        public ActionResult Details(string id)
        {
            
            return View(dbcon.GetUserDataByID(id));
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Edit(string id)
        {

            return View(dbcon.GetUserDataByID(id));
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(AspNetUser user)
        {
            if (ModelState.IsValid)
            {
                dbcon.UpdateUserInfo(user);
            }
            return View();
        }
        [Authorize(Roles = "admin,moder")]
        public ActionResult ShowEditRequests()
        {


            return View(dbcon.ShowEditRequests());
        }

        [Authorize]
        public ActionResult AddEditRequest(string id)
        {
           int c =  dbcon.AddEditRequest(id);
            if(c > 0)
            {
                ViewBag.Request = "Already Sent!";
            }
            else
            {
                ViewBag.Request = "Sent!";
            }

            return View(); 

        }
        [Authorize(Roles = "admin,moder")]
        public ActionResult AcceptUserEditRequest(string id)
        {
            dbcon.AcceptUserEditRequest(id);
            return RedirectToAction("ShowEditRequests","Users") ;
        }
        [Authorize(Roles = "admin,moder")]
        public ActionResult DeclineEditRequest(string id)
        {
            dbcon.DeclineSpotEditRequest(id);
            return RedirectToAction("ShowEditRequests", "Users");
        }

        [Authorize(Roles = "admin,moder")]
        public ActionResult ShowActions()
        {



            return View(dbcon.GetAllUserActions());
        }

    }
}