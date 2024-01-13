using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class StoryChapterViewDTO
    {
        public string storyName { get; set; }
        public string storySlug { get; set; }
        public string title { get; set; }
        public string preChapterSlug { get; set; }
        public string nextChapterSlug { get; set; }
        public string content { get; set; }
        public List<StoryChapterDTO> storyChapters { get; set; }
    }
}