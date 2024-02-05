using DatabaseProvider;
using DBIO;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;

namespace WebsiteDocTruyenChu.Controllers
{
    public class HomeController : Controller
    {
        MyDB mydb = new MyDB();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string username, string password)
        {
            JsonResult result = new JsonResult();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var errorObj = new ErrorResponse(400, "Vui lòng cung cấp đầy đủ thông tin cần thiết");
                Response.StatusCode = errorObj.StatusCode;
                result.Data = errorObj;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var existedUser = mydb.GetUserByUserName(username);
                    if (existedUser == null)
                    {
                        var errorObj = new ErrorResponse(401);
                        Response.StatusCode = errorObj.StatusCode;
                        result.Data = errorObj;
                    }
                    else
                    {
                        var hashPassword = StaticMethods.GetMD5(password);
                        if (existedUser.hashPwd != hashPassword)
                        {
                            var errorObj = new ErrorResponse(401, "Tài khoản hoặc mật khẩu không chính xác");
                            Response.StatusCode = errorObj.StatusCode;
                            result.Data = errorObj;
                        }
                        else
                        {
                            Session["user"] = new UserDTO()
                            {
                                userID = existedUser.uid,
                                Username = existedUser.username,
                                FullName = existedUser.fullname,
                                Role = existedUser.role,
                            };
                            //Session.Timeout = 10;
                            result.Data = new Models.Response()
                            {
                                message = "Đăng nhập thành công",
                                data = new
                                {
                                    userID = existedUser.uid,
                                    username = existedUser.username,
                                    fullname = existedUser.fullname
                                }
                            };
                        }
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(string username, string password, string confirmPassword, string fullname)
        {
            JsonResult result = new JsonResult();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(fullname))
            {
                var errorObj = new ErrorResponse(400, "Vui lòng cung cấp đầy đủ thông tin cần thiết");
                Response.StatusCode = errorObj.StatusCode;
                result.Data = errorObj;
            }
            else if (confirmPassword != password)
            {
                var errorObj = new ErrorResponse(400, "Nhập lại mật khẩu không khớp");
                Response.StatusCode = errorObj.StatusCode;
                result.Data = errorObj;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var existedUser = mydb.GetUserByUserName(username);
                    if (existedUser != null)
                    {
                        var errorObj = new ErrorResponse(400, "Tên tài khoản đã tồn tại");
                        Response.StatusCode = errorObj.StatusCode;
                        result.Data = errorObj;
                    }
                    else
                    {
                        var hashPassword = StaticMethods.GetMD5(password);
                        mydb.AddRecord(new User()
                        {
                            username = username,
                            password = password,
                            hashPwd = hashPassword,
                            fullname = fullname,
                            role = StaticVariables.ROLE_USER,
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now
                        });
                        mydb.AddRecord(new UserDetail()
                        {
                            username = username,
                            friends = "[]",
                            followers = "[]",
                            followings = "[]",
                            favourites = "[]",
                            avatar = null,
                            bio = null
                        });
                        mydb.SaveChanges();
                        var user = mydb.GetUserByUserName(username);
                        Session["user"] = new UserDTO()
                        {
                            Username = user.username,
                            FullName = user.fullname,
                            Role = existedUser.role,
                        };
                        //Session.Timeout = 10;
                        result.Data = new Models.Response()
                        {
                            message = "Đăng ký thành công",
                            data = new
                            {
                                userID = user.uid,
                                fullname = user.fullname,
                                username = user.username
                            }
                        };
                    }
                }
            }
            return Json(result);
        }

        public JsonResult Logout()
        {
            if (Session["user"] != null)
            {
                Session.Remove("user");
            }
            return Json(new Models.Response()
            {
                message = "Đã đăng xuất"
            }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            Session["routeName"] = "Home";
            ViewBag.Title = "Đọc truyện chữ online";
            var hotStories = mydb.GetHotStories().Take(16).Select(s => new HomeStoryImageDTO
            {
                name = s.name,
                slug = s.slug,
                createdAt = s.createdAt,
                insideImage = s.insideImage,
                isHot = s.isHot,
                status = s.status,
            }).ToList();
            List<Category> categories = mydb.GetCategories();
            var newestStories = mydb.GetStoriesOrderByField("updatedAt", 1).Take(30).Select(s => new HomeStoryNoImageDTO
            {
                name = s.name,
                slug = s.slug,
                createdAt = s.createdAt,
                isHot = s.isHot,
                genres = s.genres,
                status = s.status,
                lastChapter = s.lastChapter,
                lastChapterSlug = s.lastChapterSlug
            }).ToList();
            var fullStories = mydb.GetStoriesByStatus("Full").Take(16).Select(s => new HomeStoryImageDTO
            {
                name = s.name,
                slug = s.slug,
                createdAt = s.createdAt,
                insideImage = s.insideImage,
                isHot = s.isHot,
                status = s.status,
                lastChapterSlug = s.lastChapterSlug
            }).ToList();
            ViewModelWithFourParams<List<HomeStoryImageDTO>, List<Category>, List<HomeStoryNoImageDTO>, List<HomeStoryImageDTO>> ViewModel =
                new ViewModelWithFourParams<List<HomeStoryImageDTO>, List<Category>, List<HomeStoryNoImageDTO>, List<HomeStoryImageDTO>>
                {
                    Item1 = hotStories,
                    Item2 = categories,
                    Item3 = newestStories,
                    Item4 = fullStories,
                };
            return View(ViewModel);
        }

        // GET: /loc/truyen-hot/:categorySlug
        public JsonResult FilterHotStories(string slug)
        {
            var hotStories = slug == "all" ? mydb.GetHotStories().Take(16).ToList() : mydb.GetHotStories(slug).Take(16).ToList();
            string html = "";
            if (hotStories.Count > 0)
            {
                foreach (Story hotStory in hotStories)
                {
                    html += "<div class=\"story-item\">";
                    html += "<a href=\"/doc-truyen/" + hotStory.slug + "\" class=\"d-block text-decoration-none\">";
                    html += "<div class=\"story-item__image\">";
                    html += "<img src=\"" + hotStory.insideImage + "\" alt=\"" + hotStory.name + "\" class=\"img-fluid\" width=\"150\" height=\"230\" loading=\"lazy\">";
                    html += "</div>";
                    html += "<h3 class=\"story-item__name text-one-row story-name\">" + hotStory.name + "</h3>";
                    html += "<div class=\"list-badge\">";
                    if (hotStory.status.ToLower() == "full")
                    {
                        html += "<span class=\"story-item__badge badge text-bg-success\">Full</span>";
                    }
                    html += "<span class=\"story-item__badge story-item__badge-hot badge text-bg-danger\">Hot</span>";
                    //html += "<span class=\"story-item__badge story-item__badge-new badge text-bg-info text-light\">New</span>";
                    html += "</div>";
                    html += "</a>";
                    html += "</div>";
                }
            }
            else
            {
                html += "<div>Không có truyện nào!</div>";
            }
            Response res = new Response();
            res.message = "get hot stories by slug: [" + slug + "] successful";
            res.html = html;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // tim-kiem/:payload
        public JsonResult SearchStories(string payload)
        {
            Response res = new Response();
            string html = "";
            res.message = "get stories by name: [" + payload + "] successful";
            var stories = mydb.GetStories().Where(s => s.name.Contains(payload)).Take(5).ToList();
            stories.ForEach(s =>
            {
                html += "<li class='list-group-item'>";
                html += "<a href='/doc-truyen/" + s.slug + "' class='text-dark hover-title'>" + s.name + "</a>";
                html += "</li>";
            });
            res.html = html;
            //res.listStory = stories;
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}