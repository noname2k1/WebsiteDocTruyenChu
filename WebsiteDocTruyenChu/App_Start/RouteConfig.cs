using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteDocTruyenChu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "admin tables",
             url: "admin/{type}",
             defaults: new { controller = "Admin", action = "Manager"}
            );

            routes.MapRoute(
             name: "table - get more items",
             url: "service/get-more-items/{type}",
             defaults: new { controller = "Service", action = "GetMoreItems" }
            );

            routes.MapRoute(
             name: "global chat",
             url: "message/{action}/{id}",
             defaults: new { controller = "Message", action = "Index" }
            );

            routes.MapRoute(
              name: "Category",
              url: "the-loai/{slug}",
              defaults: new { controller = "Page", action = "Category" }
            );

            routes.MapRoute(
             name: "Category (full)",
             url: "the-loai/{slug}/hoan",
             defaults: new { controller = "Page", action = "Category" }
            );

            routes.MapRoute(
                name: "seach-by-name",
                url: "tim-kiem/{payload}",
                defaults: new { controller = "Home", action = "SearchStories" }
            );

            routes.MapRoute(
                name: "Author",
                url: "tac-gia/{slug}",
                defaults: new { controller = "Home", action = "SearchResult" }
            );

            routes.MapRoute(
                name: "Search result",
                url: "ket-qua/{payload}",
                defaults: new { controller = "Home", action = "SearchResult" }
            );

            routes.MapRoute(
              name: "list",
              url: "danh-sach/{slug}",
              defaults: new { controller = "page", action = "Category" }
           );

            routes.MapRoute(
             name: "filter stories by chapter count",
             url: "top-truyen/{slug}",
             defaults: new { controller = "Page", action = "Category" }
            );

            routes.MapRoute(
              name: "Story",
              url: "doc-truyen/{storySlug}",
              defaults: new { controller = "Page", action = "Story" }
            );

            routes.MapRoute(
              name: "StoryChapter",
              url: "doc-truyen/{storySlug}/{chapterSlug}",
              defaults: new { controller = "Page", action = "StoryChapter" }
            );

            routes.MapRoute(
                name: "FilterHotStories",
                url: "loc/truyen-hot/{slug}",
                defaults: new { controller = "Home", action = "FilterHotStories" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
