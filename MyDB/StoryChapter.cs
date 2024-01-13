namespace MyDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StoryChapter")]
    public partial class StoryChapter
    {
        public int storyChapterID { get; set; }

        [StringLength(50)]
        public string storySlug { get; set; }

        [StringLength(50)]
        public string storyName { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(50)]
        public string slug { get; set; }

        [StringLength(50)]
        public string preChapterSlug { get; set; }

        [StringLength(50)]
        public string nextChapterSlug { get; set; }

        public string content { get; set; }
    }
}
