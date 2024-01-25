using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class MessageDTO
    {
        public int MessageID { get; set; }
        public string Fullname { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}