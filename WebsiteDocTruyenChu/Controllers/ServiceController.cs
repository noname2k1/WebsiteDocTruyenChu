using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBIO;
using WebsiteDocTruyenChu.DTOs;
using WebsiteDocTruyenChu.Helpers;
using WebsiteDocTruyenChu.Models;
using System.Linq.Dynamic;
using DatabaseProvider;
using System.IO;
using Newtonsoft.Json;

namespace WebsiteDocTruyenChu.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 60)]
        public JsonResult GetChapterCountsByMonth(int year, int month)
        {
            DataController Dc = new DataController();
            JsonResult Jr = new JsonResult();
            DateTime now = new DateTime();
            if (year < 0 && year > now.Year)
            {
                ErrorResponse errorObj = new ErrorResponse(400, "Invalid Year");
                Response.StatusCode = errorObj.StatusCode;
                Jr.Data = errorObj;
            }
            else
            {
                var chapterCount = Dc.GetChapterQuantityGroupByMonthOfTheYear(year, month);
                Response res = new Response()
                {
                    data = chapterCount,
                    message = string.Format("Get ChapterCountByMonth {0}/{1} success", month.ToString(), year.ToString())
                };
                Jr.Data = res;
            }
            return Json(Jr);
        }

        [OutputCache(Duration = 120)]
        public JsonResult GetCategories()
        {
            MyDB myDB = new MyDB();
            JsonResult Jr = new JsonResult();
            Response res = new Response()
            {
                data = myDB.GetCategories(),
                message = "Get categories success"
            };
            Jr.Data = res;
            return Json(Jr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(Duration = 60)]
        public JsonResult GetStoryCountByCategory(string categorySlug)
        {
            DataController Dc = new DataController();
            JsonResult Jr = new JsonResult();
            var storyCount = Dc.GetStoryCountByCategory(categorySlug);
            Response res = new Response()
            {
                data = storyCount,
                message = string.Format("Get Story Count by Category success")
            };
            Jr.Data = res;
            return Json(Jr);
        }

        public class DataTablesRequest
        {
            public int draw { get; set; }
            public List<Column> columns { get; set; }
            public List<Order> order { get; set; }
            public int start { get; set; }
            public int length { get; set; }
            public Search search { get; set; }
        }

        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }

        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }

        public class Search
        {
            public string value { get; set; }
            public bool regex { get; set; }
        }

        [HttpPost]
        public JsonResult GetMoreItems(string type)
        {
            MyDB myDB = new MyDB();

            DataTablesRequest request = StaticMethods.RequestBodyConverter<DataTablesRequest>(Request);

            var draw = request.draw;
            var sortColumn = request.columns[request.order[0].column].name;
            var sortColumnDir = request.order[0].dir;
            var searchValue = request.search.value;
            var query = Request.QueryString["query"];

            //Paging Size (10,20,50,100)
            int pageSize = request.length;
            int skip = request.start;
            int recordsTotal = 0;

            //Returning Json Data
            var data = new List<dynamic>();
            switch (type.ToLower())
            {
                case "users":
                    recordsTotal = myDB.GetUsers().Count();
                    var tempUsers = myDB.GetUsers();
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        tempUsers = tempUsers.Where(u => u.uid.ToString() == searchValue || u.username.Contains(searchValue) || u.fullname.Contains(searchValue)
                        || u.hashPwd.Contains(searchValue) || u.password.Contains(searchValue)
                        );
                    }
                    //Sorting
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        tempUsers = tempUsers.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    else
                    {
                        tempUsers = tempUsers.OrderBy(u => u.uid);
                    }
                    //Paging
                    data = tempUsers.Skip(skip).Take(pageSize).ToList<dynamic>();
                    break;
                case "user-detail":
                    recordsTotal = myDB.GetUserDetails().Count();
                    var tempUserDetail = myDB.GetUserDetails();
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        tempUserDetail = tempUserDetail.Where(u => u.udID.ToString() == searchValue || u.username.Contains(searchValue));
                    }
                    //Sorting
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        tempUserDetail = tempUserDetail.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    else
                    {
                        tempUserDetail = tempUserDetail.OrderBy(u => u.udID);
                    }
                    if (!String.IsNullOrEmpty(query))
                    {
                        tempUserDetail = tempUserDetail.Where(u => u.username == query);
                    }
                    //Paging
                    data = tempUserDetail.Skip(skip).Take(pageSize).ToList<dynamic>();
                    break;
                case "categories":
                    recordsTotal = myDB.GetCategories2().Count();
                    var tempCategories = myDB.GetCategories2();
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        tempCategories = tempCategories.Where(c => c.categoryID.ToString() == searchValue || c.categoryName.Contains(searchValue) || c.description.Contains(searchValue)
                        || c.path.Contains(searchValue) || c.value.ToString() == searchValue
                        );
                    }
                    //Sorting
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        tempCategories = tempCategories.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    else
                    {
                        tempCategories = tempCategories.OrderBy(u => u.categoryID);
                    }
                    //Paging
                    data = tempCategories.Skip(skip).Take(pageSize).ToList<dynamic>();
                    break;
                case "stories":
                    if (!String.IsNullOrEmpty(query))
                    {
                        var storySlug = query;
                        recordsTotal = myDB.GetChapters(storySlug).Count();
                        var tempChapters = myDB.GetChapters(storySlug);
                        //Search
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            string authorName = myDB.GetAuthor(searchValue) != null ? myDB.GetAuthor(searchValue).name : "";
                            tempChapters = tempChapters.Where(s => s.storyChapterID.ToString() == searchValue || s.title.Contains(searchValue)
                            || s.storyName.Contains(searchValue) || s.storySlug.ToString() == searchValue || s.content.Contains(searchValue));
                        }
                        //Sorting
                        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                        {
                            tempChapters = tempChapters.OrderBy(sortColumn + " " + sortColumnDir);
                        }
                        else
                        {
                            tempChapters = tempChapters.OrderBy(c => c.storyChapterID);
                        }
                        //Paging
                        data = tempChapters.Skip(skip).Take(pageSize).ToList<dynamic>();
                    }
                    else
                    {
                        recordsTotal = myDB.GetStories().Count();
                        var tempStories = myDB.GetStories();
                        //Search
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            string authorName = myDB.GetAuthor(searchValue) != null ? myDB.GetAuthor(searchValue).name : "";
                            tempStories = tempStories.Where(s => s.storyID.ToString() == searchValue || s.name.Contains(searchValue) || authorName.Contains(searchValue)
                            || s.description.Contains(searchValue) || s.rateCount.ToString() == searchValue || s.rateScore.ToString() == searchValue || s.isHot.ToString() == searchValue
                            || s.slug.Contains(searchValue) || s.genres.Contains(searchValue) || s.status.Contains(searchValue) || s.lastChapter.Contains(searchValue)
                            );
                        }
                        //Sorting
                        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                        {
                            tempStories = tempStories.OrderBy(sortColumn + " " + sortColumnDir);
                        }
                        else
                        {
                            tempStories = tempStories.OrderBy(u => u.storyID);
                        }
                        //Paging
                        var stories = tempStories.Skip(skip).Take(pageSize).ToList<dynamic>();
                        foreach (var story in stories.ToList())
                        {
                            data.Add(new AdminStoryDTO()
                            {
                                storyID = story.storyID,
                                name = story.name,
                                slug = story.slug,
                                author = myDB.GetAuthor(story.author).name,
                                coverImage = story.coverImage,
                                insideImage = story.insideImage,
                                lastChapter = story.lastChapter,
                                lastChapterSlug = story.lastChapterSlug,
                                status = story.status,
                                isHot = story.isHot,
                                genres = story.genres,
                                rateCount = story.rateCount,
                                rateScore = story.rateScore,
                                description = story.description,
                                createdAt = story.createdAt,
                                updatedAt = story.updatedAt
                            }); ;
                        }
                    }
                    break;
                case "rooms":
                    if (!string.IsNullOrEmpty(query))
                    {
                        recordsTotal = myDB.GetRooms().Count();
                        var tempMessages = myDB.GetMessages(Convert.ToInt32(query));
                        //Search
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            tempMessages = tempMessages.Where(m => m.MESSAGEID.ToString() == searchValue || m.roomID.ToString() == searchValue || m.content.Contains(searchValue)
                            || m.userid.ToString().Contains(searchValue)
                            );
                        }
                        //Sorting
                        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                        {
                            tempMessages = tempMessages.OrderBy(sortColumn + " " + sortColumnDir);
                        }
                        else
                        {
                            tempMessages = tempMessages.OrderBy(u => u.MESSAGEID);
                        }
                        //Paging
                        data = tempMessages.Skip(skip).Take(pageSize).ToList<dynamic>();
                    }
                    else
                    {
                        recordsTotal = myDB.GetRooms().Count();
                        var tempRooms = myDB.GetRooms();
                        //Search
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            tempRooms = tempRooms.Where(r => r.roomID.ToString() == searchValue || r.roomName.Contains(searchValue) || r.type.ToString() == searchValue);
                        }
                        //Sorting
                        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                        {
                            tempRooms = tempRooms.OrderBy(sortColumn + " " + sortColumnDir);
                        }
                        else
                        {
                            tempRooms = tempRooms.OrderBy(u => u.roomID);
                        }
                        //Paging
                        var Rooms = tempRooms.Skip(skip).Take(pageSize).ToList();
                        foreach (var Room in Rooms)
                        {
                            int MessageCount = myDB.GetMessages(Room.roomID).Count();
                            data.Add(new AdminRoomDTO()
                            {
                                ID = Room.roomID,
                                Name = Room.roomName,
                                CreatedAt = Room.createdAt,
                                UpdatedAt = Room.updatedAt,
                                DeletedAt = Room.deletedAt,
                                MessageCount = MessageCount,
                            });
                        }
                    }
                    break;
            }

            return Json(new ResponseDatatablesJquery()
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = data,
            });

        }

        [ActionName("authors")]
        public JsonResult GetAuthors()
        {
            var JR = new JsonResult();
            JR.Data = new Response()
            {
                message = "Get authors successfully",
                data = new MyDB().GetAuthors().Select(a => new
                {
                    a.name,
                    a.slug
                }).ToList()
            };
            return Json(JR, JsonRequestBehavior.AllowGet);
        }
    }
}