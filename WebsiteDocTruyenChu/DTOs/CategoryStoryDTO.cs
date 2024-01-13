using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class CategoryStoryDTO
    {
        public string name { get; set; }
        public string coverImage { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public bool isHot { get; set; }
        public string author { get; set; }
        public string lastChapter { get; set; }
        public string lastChapterSlug { get; set; }
    }
}