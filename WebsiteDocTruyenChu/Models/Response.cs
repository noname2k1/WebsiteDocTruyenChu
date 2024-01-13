using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class Response
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string html { get; set; }

        public List<Story> listStory { get; set; }
    }
}