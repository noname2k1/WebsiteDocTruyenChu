namespace MyDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Author")]
    public partial class Author
    {
        public int authorID { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string slug { get; set; }
    }
}
