namespace DatabaseProvider
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Story")]
    public partial class Story
    {
        public int storyID { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string author { get; set; }

        [StringLength(255)]
        public string coverImage { get; set; }

        [StringLength(255)]
        public string insideImage { get; set; }

        [StringLength(255)]
        public string lastChapter { get; set; }

        [StringLength(255)]
        public string lastChapterSlug { get; set; }

        [StringLength(255)]
        public string status { get; set; }

        [StringLength(255)]
        public string genres { get; set; }

        public int? rateCount { get; set; }

        public double? rateScore { get; set; }

        public string description { get; set; }

        [StringLength(255)]
        public string slug { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        public DateTime? deletedAt { get; set; }

        public bool isHot { get; set; }
    }
}
