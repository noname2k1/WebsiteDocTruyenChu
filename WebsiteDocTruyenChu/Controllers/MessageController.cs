
using DatabaseProvider;
using DBIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteDocTruyenChu.Controllers
{
    public class MessageController : Controller
    {
        MyDB myDB = new MyDB();
        // GET: Message
        public ActionResult Index()
        {
            Session["routeName"] = "ChatGlobal";
            ViewBag.Title = "Chat Global";
            IQueryable<Room> OwnRooms = myDB.GetRooms().Where(r => r.type == 0 || r.type == 1);
            return View();
        }

        [HttpPost]
        public ActionResult New()
        {
            Session["routeName"] = "ChatGlobal";
            ViewBag.Title = "Chat Global";
            return View();
        }
    }
}