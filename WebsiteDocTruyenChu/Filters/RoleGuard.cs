using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebsiteDocTruyenChu.DTOs;

namespace WebsiteDocTruyenChu.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RoleGuard : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var myUser = (UserDTO)filterContext.HttpContext.Session["user"];
            if (myUser == null || !Roles.Split(',').Contains(myUser.Role.ToString()))
            {
                if(myUser != null)
                {
                    filterContext.HttpContext.Session.Remove("user");
                }
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Login");
                redirectTargetDictionary.Add("controller", "Admin");
                redirectTargetDictionary.Add("area", "");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}