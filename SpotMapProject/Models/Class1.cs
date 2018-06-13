using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace SpotMapProject.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            /*context.Roles.Add(new IdentityRole { Name = "admin" });
            context.SaveChanges();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

    
            var role1 = new IdentityRole { Name = "Admin" };
            var role2 = new IdentityRole { Name = "User" };

         
            roleManager.Create(role1);
            roleManager.Create(role2);

     
            var admin = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
            string password = "ad46D_ewr3";
            var result = userManager.Create(admin, password);

        
            if (result.Succeeded)
            {
              
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            base.Seed(context);
            */
           // System.Web.Security.Roles.AddUserToRole("galavewka1@gmail.com", "admin");
        }
    }
}
