using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class AdminRoomDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int MessageCount { get; set; }
    }
}