namespace DatabaseProvider
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("userDetail")]
    public partial class UserDetail
    {
        [Key]
        public int udID { get; set; }

        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [StringLength(255)]
        public string favourites { get; set; }

        [Required]
        [StringLength(255)]
        public string followers { get; set; }

        [Required]
        [StringLength(255)]
        public string followings { get; set; }

        [Required]
        [StringLength(255)]
        public string friends { get; set; }

        [StringLength(255)]
        public string avatar { get; set; }

        public string bio { get; set; }
    }
}
