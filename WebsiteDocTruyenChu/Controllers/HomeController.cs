using DatabaseProvider;
using DBIO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Models;

namespace WebsiteDocTruyenChu.Controllers
{
    public class HomeController : Controller
    {
        MyDB mydb = new MyDB();
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
            res.success = true;
            res.message = "get hot stories by slug: [" + slug + "] successful";
            res.html = html;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // tim-kiem/:payload
        public JsonResult SearchStories(string payload)
        {
            Response res = new Response();
            string html = "";
            res.success = true;
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