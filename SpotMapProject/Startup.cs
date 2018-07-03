using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security;
using SpotMapProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

[assembly: OwinStartupAttribute(typeof(SpotMapProject.Startup))]
namespace SpotMapProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //  var _context = new ApplicationDbContext();
            // var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            //  UserManager.AddToRole("fe781eaa-a9a0-44d6-89f1-d487023cc81a", "admin");
            //Roles.RemoveUserFromRole();
            app.MapSignalR();

        }
    }
}
