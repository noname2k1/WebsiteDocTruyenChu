namespace DatabaseProvider
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CATEGORIES")]
    public partial class Category
    {
        public int categoryID { get; set; }

        [StringLength(30)]
        public string categoryName { get; set; }

        public int? value { get; set; }

        [StringLength(100)]
        public string path { get; set; }

        [StringLength(255)]
        public string description { get; set; }
    }
}
