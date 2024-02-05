using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIO
{
    public class DataController
    {
        private Model model = new Model();

        public int GetChapterQuantityGroupByMonthOfTheYear(int year, int month)
        {
            return model.StoryChapters.Count(s => s.createdAt.Year == year && s.createdAt.Month == month);
        }

        public int GetStoryCountByCategory(string categorySlug)
        {
            return model.Stories.Where(s => s.genres.Contains(categorySlug)).Count();
        }
    }
}
