
using DatabaseProvider;
using DBIO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;

namespace WebsiteDocTruyenChu.Controllers
{
    public class MessageController : Controller
    {
        MyDB myDB = new MyDB();
        // GET: Message
        public ActionResult Index()
        {
            Session["routeName"] = "chat";
            ViewBag.Title = "Trao đổi tin tức";
            var viewModel = new ViewModelTwoParams<User, Room>();
            Room GlobalRoom = myDB.GetRooms().Where(r => r.type == StaticVariables.TYPE_MESSAGE_GLOBAL).FirstOrDefault();
            if (GlobalRoom == null)
            {
                myDB.AddRecord(new Room()
                {
                    roomName = "GLOBAL_UNIQUE",
                    type = StaticVariables.TYPE_MESSAGE_GLOBAL,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                }
                );
                myDB.SaveChanges();
                viewModel.Item2 = myDB.GetRooms().Where(r => r.type == StaticVariables.TYPE_MESSAGE_GLOBAL).FirstOrDefault();
            }
            else
            {
                Model model = new Model();
                viewModel.Item2 = GlobalRoom;
                var query = from m in model.Messages
                            join u in model.Users on m.userid equals u.uid
                            where m.roomID == GlobalRoom.roomID
                            select new MessageDTO()
                            {
                                MessageID = m.MESSAGEID,
                                Fullname = u.fullname,
                                Content = m.content,
                                CreatedAt = m.createdAt,
                            };
                ViewBag.Messages = query.OrderByDescending(m=>m.CreatedAt).Take(15).OrderBy(m=>m.CreatedAt).ToList();
            }
            User userModel = new User();
            viewModel.Item1 = userModel;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult New()
        {
            ViewBag.Title = "Chat Global";
            return Json(new JsonResult { Data = "ok" });
        }
    }
}