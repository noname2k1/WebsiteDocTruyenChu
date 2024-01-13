using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Helpers
{
    public class StaticVariables
    {
        public static List<Models.ListModel> getListItems()
        {
            List<Models.ListModel> links = new List<Models.ListModel>();
            links.Add(new Models.ListModel("Truyện Mới Cập Nhật", "truyen-moi-cap-nhat"));
            links.Add(new Models.ListModel("Truyện Hot", "truyen-hot"));
            links.Add(new Models.ListModel("Truyện Full", "truyen-full"));
            return links;
        }
        public static List<Models.ListModel> getFilterByChapterCount()
        {
            List<Models.ListModel> links = new List<Models.ListModel>();
            links.Add(new Models.ListModel("Dưới 100", "duoi-100-chuong"));
            links.Add(new Models.ListModel("100 - 500", "100-500-chuong"));
            links.Add(new Models.ListModel("500 - 1000", "500-1000-chuong"));
            links.Add(new Models.ListModel("Trên 1000", "tren-1000-chuong"));
            return links;
        }

    }
}