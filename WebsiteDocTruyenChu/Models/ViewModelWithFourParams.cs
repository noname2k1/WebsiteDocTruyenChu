using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class ViewModelWithFourParams<A, B, C, D>
    {
        public A Item1 { get; set; }
        public B Item2 { get; set; }
        public C Item3 { get; set; }
        public D Item4 { get; set; }
    }
}