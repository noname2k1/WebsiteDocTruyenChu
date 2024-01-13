using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class ViewModelTwoParams<T, K>
    {
        public T Item1 { get; set; }
        public K Item2 { get; set; }
    }
}