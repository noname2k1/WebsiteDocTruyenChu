using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.DTOs
{
    public class ResponseDatatablesJquery
    {
        public int draw { get; set; }
        public List<dynamic> data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
}