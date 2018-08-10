using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpotMapProject.Models;
namespace SpotMapProject.Controllers
{
    public class ChatRoomController : Controller
    {
        private DataBaseController dbcon = new DataBaseController();
        // GET: ChatRoom
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMessages()
        {

            List<AspNetChatMessage> msg= new List<AspNetChatMessage>();
           msg = dbcon.GetChatMessages();
            var jsonResult = Json(msg, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}