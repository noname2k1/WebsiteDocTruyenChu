using DatabaseProvider;
using DBIO;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;
using WebsiteDocTruyenChu.Models.Admin;

namespace WebsiteDocTruyenChu.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        MyDB myDB = new MyDB();
        public ActionResult Index()
        {
            //var myUser = (UserDTO)Session["user"];
            //if (myUser == null || !StaticMethods.CheckAdminPageAccess(myUser))
            //{
            //    return RedirectToAction("Login");
            //}
            var CategoryCount = myDB.GetCategories().Count();
            var StoryCount = myDB.GetStories().Count();
            var UserCount = myDB.GetUsers().Count();
            var RoomCount = myDB.GetRooms().Count();
            var ViewModel = new IndexModel()
            {
                CategoryCount = CategoryCount,
                StoryCount = StoryCount,
                UserCount = UserCount,
                RoomCount = RoomCount
            };
            return View(ViewModel);
        }

        public ActionResult Login()
        {
            var myUser = (UserDTO)Session["user"];
            if (myUser != null && StaticMethods.CheckAdminPageAccess(myUser))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult StayOrBye(int userID)
        {
            var result = new JsonResult();
            var myUser = (UserDTO)Session["user"];
            if (myUser != null && StaticMethods.CheckAdminPageAccess(myUser))
            {
                result.Data = new Response()
                {
                    message = "allowed",
                    data = true
                };
            }
            else
            {
                var errorObj = new ErrorResponse(403);
                Response.StatusCode = errorObj.StatusCode;
                result.Data = errorObj;
            }
            return Json(result);
        }

        // Other Pages
        public ActionResult Manager(string type)
        {
            ViewBag.Title = char.ToUpper(type.Trim()[0]) + type.Trim().Substring(1);
            List<string> tableLabels = new List<string>();
            switch (type.ToLower())
            {
                case "users":
                    tableLabels.AddRange(new[]
                    {
                        "UID",
                        "Username",
                        "Hashed Password",
                        "Raw Password",
                        "Role",
                        "Full Name",
                        "Created At",
                        "Updated At"
                    });
                    break;
                case "categories":
                    tableLabels.AddRange(new[]
                    {
                        "ID",
                        "Name",
                        "Value",
                        "Slug",
                        "Description"
                    });
                    break;
                case "rooms":
                    tableLabels.AddRange(new[]
                    {
                        "ID",
                        "Name",
                        "Created At",
                        "Updated At",
                        "Message Count"
                    });
                    break;
                case "stories":
                    tableLabels.AddRange(new[]
                    {
                        "ID",
                        "Name",
                        "Slug",
                        "Author",
                        "CoverImage",
                        "InsideImage",
                        "Status / Is Hot",
                        "Genres",
                        "Rating Count/ Score",
                    });
                    break;
            }
            ViewBag.Labels = tableLabels;
            var viewModel = new Models.ViewModelTwoParams<string, List<string>>()
            {
                Item1 = type,
                Item2 = tableLabels
            };
            return View(viewModel);
        }

    }
}