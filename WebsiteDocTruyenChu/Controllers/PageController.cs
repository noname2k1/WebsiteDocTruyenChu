using DatabaseProvider;
using DBIO;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;

namespace WebsiteDocTruyenChu.Controllers
{
    public class PageController : Controller
    {
        MyDB myDB = new MyDB();
        Model model = new Model();
        public ActionResult Index()
        {
            return View();
        }

        private void ApplyInfoToView(List<ListModel> list, string slug, bool isFull)
        {
            if (list.Any(model => model.Slug == slug))
            {
                var model = list.Find(m => m.Slug == slug);
                ViewBag.Title = model.Name + (isFull ? " FULL" : "");
                ViewBag.Slug = model.Slug;
                ViewBag.Desc = "";
                Session["routeName"] = "Category";
                Session["routeTitle"] = model.Name;
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult Category(string slug)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int limit = 30;
            bool getFull = Request.Url.AbsolutePath.Contains("hoan");
            List<ListModel> listModels = StaticVariables.getListItems();
            List<ListModel> filtersByChapterCount = StaticVariables.getFilterByChapterCount();
            IQueryable<Story> iQueryableStories;
            var authorsTask = Task.Run(() => myDB.GetAuthors().ToListAsync());
            // lọc theo danh-sach
            if (listModels.Any(model => model.Slug == slug))
            {
                ApplyInfoToView(listModels, slug, getFull);
                iQueryableStories = myDB.GetStoriesByList(slug, getFull ? "full" : "");
            }
            // lọc truyện theo số chap
            else if (filtersByChapterCount.Any(model => model.Slug == slug))
            {
                ApplyInfoToView(filtersByChapterCount, slug, getFull);
                iQueryableStories = myDB.GetStoriesByList(slug, getFull ? "full" : "");
            }
            else
            {
                // lọc truyện theo thể loại
                var Category = myDB.GetCategories().Find(category => category.path == slug);
                ViewBag.Title = Category.categoryName + (getFull ? " FULL" : "");
                ViewBag.Slug = slug;
                ViewBag.Desc = Category.description;
                Session["routeName"] = "Category";
                Session["routeTitle"] = Category.categoryName;
                iQueryableStories = myDB.GetStoriesByCategory(slug, getFull ? "full" : "");
            }
            // chung
            int page;
            if (!int.TryParse(Request.QueryString["page"], out page) || page <= 0)
            {
                page = 1;
            }
            var totalStories = iQueryableStories.Count();
            int pageCount = (int)Math.Ceiling((double)totalStories / limit);
            if (page > pageCount)
            {
                page = pageCount;
            }

            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;

            var storiesTask = Task.Run(() => iQueryableStories.Skip(page > 1 ? page : 1).Take(limit).Select(s => new CategoryStoryDTO
            {
                name = s.name,
                slug = s.slug,
                author = s.author,
                coverImage = s.coverImage,
                isHot = s.isHot,
                lastChapter = s.lastChapter,
                lastChapterSlug = s.lastChapterSlug,
                status = s.status,
            }).ToListAsync());

            Task.WhenAll(authorsTask, storiesTask);

            stopWatch.Stop();
            TimeSpan elapsedTime = stopWatch.Elapsed;
            string elapsedTimeString = elapsedTime.ToString();

            // In ra thời gian chạy
            Debug.WriteLine("Thời gian chạy: " + elapsedTimeString);

            ViewModelTwoParams<List<Author>, List<CategoryStoryDTO>> viewModel = new ViewModelTwoParams<List<Author>, List<CategoryStoryDTO>>
            {
                Item1 = authorsTask.Result,
                Item2 = storiesTask.Result
            };
            return View(viewModel);
        }

        public ActionResult Story(string storySlug)
        {
            int limit = 50;
            IQueryable<StoryChapter> iQueryableStories = myDB.GetChapters(storySlug);
            int page;
            if (!int.TryParse(Request.QueryString["page"], out page) || page <= 0)
            {
                page = 1;
            }
            var totalStories = iQueryableStories.Count();
            int pageCount = (int)Math.Ceiling((double)totalStories / limit);
            if (page > pageCount)
            {
                page = pageCount;
            }
            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;

            var storyTask = Task.Run(() => myDB.GetStories().Where(s => s.slug == storySlug.ToLower()).FirstOrDefaultAsync());
            var storyChaptersTask = Task.Run(() => iQueryableStories.OrderBy(c => c.storyChapterID).Skip(page > 1 ? page : 1).Take(limit).Select(sc => new StoryChapterDTO
            {
                slug = sc.slug,
                title = sc.title,
            }).ToListAsync());

            var author = myDB.GetAuthor(storyTask.Result.author.ToLower());
            var CategoriesTask = Task.Run(() => myDB.GetCategories());

            Task.WhenAll(storyTask, storyChaptersTask, CategoriesTask);

            Session["routeName"] = "Story";
            Session["routeTitle"] = storyTask.Result.name;
            ViewBag.Title = storyTask.Result.name;
            ViewBag.Categories = CategoriesTask.Result;
            ViewBag.AuthorName = author.name;

            ViewModelTwoParams<Story, List<StoryChapterDTO>> viewModel = new ViewModelTwoParams<Story, List<StoryChapterDTO>>
            {
                Item1 = storyTask.Result,
                Item2 = storyChaptersTask.Result
            };
            return View(viewModel);
        }

        public ActionResult StoryChapter(string storySlug, string chapterSlug)
        {
            var storyChapters = myDB.GetChapters(storySlug).OrderBy(c => c.storyChapterID).Select(sc => new StoryChapterDTO
            {
                slug = sc.slug,
                title = sc.title,
            }).ToList();
            var storyChapter = myDB.GetChapters().Where(sc => sc.slug == chapterSlug && sc.storySlug == storySlug.ToLower()).Select(sc => new StoryChapterViewDTO
            {
                storyName = sc.storyName,
                storySlug = sc.storySlug,
                content = sc.content,
                nextChapterSlug = sc.nextChapterSlug,
                preChapterSlug = sc.preChapterSlug,
                title = sc.title,
            }).FirstOrDefault();
            storyChapter.storyChapters = storyChapters;
            Session["routeName"] = "storyChapter";
            Session["routeTitle"] = storyChapter.title;
            Session["preRouteSlug"] = storyChapter.storySlug;
            Session["preRouteTitle"] = storyChapter.storyName;
            ViewBag.Title = storyChapter.storyName + " - " + storyChapter.title;
            return View(storyChapter);
        }
    }
}