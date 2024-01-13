using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class ListModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public ListModel() { }

        public ListModel(string name, string slug) { Name = name; Slug = slug; }

    }
}