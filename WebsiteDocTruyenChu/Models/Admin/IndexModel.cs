using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models.Admin
{
    public class IndexModel
    {
        public int CategoryCount { get; set; }
        public int StoryCount { get; set; }
        public int UserCount { get; set; }
        public int RoomCount { get; set; }
    }
}