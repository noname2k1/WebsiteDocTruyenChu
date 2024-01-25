using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string message { get; set; }
        public string html { get; set; }
        public object data { get; set; }

        public Response()
        {
            Success = true;
        }
    }
}