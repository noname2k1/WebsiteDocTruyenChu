using DatabaseProvider;
using DBIO;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Filters;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;
using WebsiteDocTruyenChu.Models.Admin;
using static WebsiteDocTruyenChu.Controllers.ServiceController;

namespace WebsiteDocTruyenChu.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        MyDB myDB = new MyDB();
        [RoleGuard(Roles = "0,1")]
        public ActionResult Index()
        {
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
            if (Session["user"] != null)
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
        //[RoleGuard(Roles = "0,1")] 
        // role admin: 0, moderator: 1, user: 2
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
                    if (String.IsNullOrEmpty(Request.QueryString["query"]))
                    {
                        tableLabels.AddRange(new[]
                        {
                            "ID",
                            "Name",
                            "Created At",
                            "Updated At",
                            "Message Count"
                        });
                    }
                    else
                    {
                        tableLabels.AddRange(new[]
                        {
                            "ID",
                            "UserID",
                            "Content",
                            "Created At",
                            "Updated At",
                        });
                    }
                    break;
                case "stories":
                    if (String.IsNullOrEmpty(Request.QueryString["query"]))
                    {
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
                            "Description"
                        });
                    }
                    else
                    {
                        tableLabels.AddRange(new[]
                        {
                            "ID",
                            "title",
                            "Slug",
                            "content",
                            "views",
                            "created at",
                            "updated at"
                        });
                    }
                    break;
                case "user-detail":
                    tableLabels.AddRange(new[]
                     {
                        "ID",
                        "Username",
                        "Favourites",
                        "followers",
                        "followings",
                        "friends",
                        "avatar",
                        "bio"
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

        [HttpPost]
        //[RoleGuard(Roles = "0,1")] 
        public JsonResult Add(string id)
        {
            var Jr = new JsonResult();
            switch (id.ToLower())
            {
                case "users":
                    var request = StaticMethods.RequestBodyConverter<UserModel>(Request);
                    var existedUser = myDB.GetUserByUserName(request.username);
                    if (existedUser == null)
                    {
                        myDB.AddRecord(new User()
                        {
                            username = request.username,
                            password = request.password,
                            hashPwd = StaticMethods.GetMD5(request.password),
                            fullname = request.fullname,
                            role = StaticVariables.ROLE_USER,
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now
                        });
                        myDB.AddRecord(new UserDetail()
                        {
                            username = request.username,
                            friends = "[]",
                            followers = "[]",
                            followings = "[]",
                            favourites = "[]",
                            avatar = null,
                            bio = null
                        });
                        Jr.Data = new Response()
                        {
                            message = "Add user successfuly",
                        };
                    }
                    else
                    {
                        var errorObj = new ErrorResponse(400, "User existed");
                        Jr.Data = errorObj;
                    }
                    break;
                default:
                    break;

            }
            myDB.SaveChanges();

            return Json(Jr);
        }

        [HttpPost]
        //[RoleGuard(Roles = "0")]
        public JsonResult Edit(string id)
        {
            var Jr = new JsonResult();
            switch (id.ToLower())
            {
                case "users":
                    var request = StaticMethods.RequestBodyConverter<UserModel>(Request);
                    var existedUser = myDB.GetUserByUserName(request.username);
                    if (existedUser != null)
                    {
                        try
                        {
                            existedUser.fullname = request.fullname;
                            existedUser.password = request.password;
                            existedUser.role = Convert.ToInt32(request.role);
                            existedUser.hashPwd = StaticMethods.GetMD5(request.password);
                            Jr.Data = new Response()
                            {
                                message = "Edit user successfuly",
                            };
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                    else
                    {
                        var errorObj = new ErrorResponse(400, "User doesn't exist");
                        Jr.Data = errorObj;
                    }
                    break;
                default:
                    break;

            }
            myDB.SaveChanges();

            return Json(Jr);
        }

        [HttpPost]
        //[RoleGuard(Roles = "0")]
        public JsonResult Remove(string id)
        {
            var type = id;
            var Jr = new JsonResult();
            var ID = Request.QueryString["id"];
            switch (type.ToLower())
            {
                case "users":
                    var user = myDB.GetUserByUserID(Convert.ToInt32(ID));
                    if (user != null)
                    {
                        var userDetail = myDB.GetUserDetail(user.username);
                        if (userDetail != null)
                        {
                            myDB.DeleteRecord(userDetail);
                            myDB.SaveChanges();
                        }
                        myDB.DeleteRecord(user);
                        Jr.Data = new Response()
                        {
                            message = "Remove user",
                        };
                    }
                    else
                    {
                        var errorObj = new ErrorResponse(400, "User doesn't exist");
                        Jr.Data = errorObj;
                    }
                    break;
                default: break;
            }
            myDB.SaveChanges();
            return Json(Jr);
        }
    }
}