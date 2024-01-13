using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class HomeStoryImageDTO
    {
        public string name { get; set; }
        public string insideImage { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public bool isHot { get; set; }
        public string lastChapterSlug { get; set; }
        public DateTime? createdAt { get; set; }
    }
}