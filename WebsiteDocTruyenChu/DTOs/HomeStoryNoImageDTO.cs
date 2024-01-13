using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class HomeStoryNoImageDTO : HomeStoryImageDTO
    {
        public string genres { get; set; }
        public string lastChapter { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}