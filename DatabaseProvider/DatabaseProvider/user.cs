namespace DatabaseProvider
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [Key]
        public int uid { get; set; }

        [StringLength(20)]
        public string username { get; set; }

        [StringLength(255)]
        public string hashPwd { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        public int? role { get; set; }

        [StringLength(100)]
        public string fullname { get; set; }
    }
}
