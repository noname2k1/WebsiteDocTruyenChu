using DatabaseProvider;
using DBIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebsiteDocTruyenChu.Helpers;

namespace WebsiteDocTruyenChu.Controllers
{
    public class PartialController : Controller
    {
        MyDB mydb = new MyDB();

        // GET: Partial
        public ActionResult _Header()
        {
            List<Category> categories = mydb.GetCategories();
            List<Models.ListModel> listItems = StaticVariables.getListItems();
            ViewBag.FilterItemByChapterCount = StaticVariables.getFilterByChapterCount();
            var data = new Tuple<List<Category>, List<Models.ListModel>>(categories, listItems);
            return PartialView(data);
        }

        public ActionResult _Footer()
        {
            List<Models.ListModel> listItems = StaticVariables.getListItems();
            return PartialView(listItems);
        }

        public ActionResult _TopStoriesPane()
        {
            return PartialView();
        }

        public ActionResult _CategoriesPane()
        {
            List<Category> categories = mydb.GetCategories();
            return PartialView(categories);
        }
    }
}