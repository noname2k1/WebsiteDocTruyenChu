using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models.Admin
{
    public class UserModel
    {
        public string uid { get; set; }

        public string username { get; set; }

        public string Email { get; set; }

        public string password { get; set; }

        public string hashPwd { get; set; }

        public string role { get; set; }

        public string fullname { get; set; }
    }
}