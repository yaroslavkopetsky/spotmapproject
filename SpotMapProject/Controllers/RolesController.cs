using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpotMapProject.Models;
using SpotMapProject.Controllers;
using Microsoft.AspNet.Identity.Owin;

namespace SpotMapProject.Controllers
{
    [Authorize(Roles = "admin,moder")]
    public class RolesController : Controller
    {
        private DataBaseController dbcon = new DataBaseController();
   

        // GET: Roles
        public ActionResult Index()
        {

            int request_amount = dbcon.ShowEditRequests().Count();
            ViewBag.EditRequests = request_amount;
            ViewBag.SpotRequests = dbcon.GetSpotRequests().Count();
            var context = new ApplicationDbContext();
            var roles = context.Roles.ToList();
            return View(roles);
        }
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var context = new ApplicationDbContext();

                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            // var account = new AccountController();
            //account.UserManager.AddToRole(user.Id, RoleName);
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
              UserManager.AddToRole(user.Id, RoleName);
            ViewBag.ResultMessage = "Role created successfully !";

         
           

            return View("ManageUserRoles");
        }
        public ActionResult ManageUserRoles()
        {
            var context = new ApplicationDbContext();
          
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>

           new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }
      
        public ActionResult Edit(string roleName)
        {
            var context = new ApplicationDbContext();
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            var context = new ApplicationDbContext();
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetRoles(string UserName)
        {
           
            var roleManager = Request.GetOwinContext().Get<RoleManager<IdentityRole>>();
            var context = new ApplicationDbContext();

            string[] rolesArray;
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                rolesArray = Roles.GetRolesForUser();
                /*
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var account = new AccountController();

                ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

             */

               
                var list = rolesArray.ToList();
                ViewBag.Roles = list;
  
            



                
            }

            return View("ManageUserRoles");
        }

        public ActionResult Delete(string RoleName)
        {
            var context = new ApplicationDbContext();
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            //Remove Role Logic
            context.Roles.Remove(thisRole);
            context.SaveChanges();
          return   RedirectToAction("Index", "Roles");
        }


        public ActionResult ShowSpots()
        {
            if (dbcon.GetAllSpots() == null)
            {
                return View("Error");
            }
            return View(dbcon.GetAllSpots());
        }


    }
}