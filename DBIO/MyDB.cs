using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace DBIO
{
    public class MyDB
    {
        private Model model = new Model();

        // category
        public List<Category> GetCategories()
        {
            return model.Categories.ToList();
        }

        // author
        public IQueryable<Author> GetAuthors()
        {
            return model.Authors;
        }

        public Author GetAuthor(string authorSlug)
        {
            return model.Authors.Where(author => author.slug == authorSlug.ToLower()).FirstOrDefault();
        }

        // story
        public IQueryable<Story> GetStories()
        {
            return model.Stories;
        }

        public IQueryable<Story> GetStoriesByStatus(string status)
        {
            return model.Stories.Where(story => story.status.ToLower() == status.ToLower());
        }

        public IQueryable<Story> GetHotStories()
        {
            return model.Stories.Where(story => story.isHot);
        }

        public IQueryable<Story> GetHotStories(string categorySlug)
        {
            return model.Stories.Where(story => story.isHot && story.genres.Contains(categorySlug.ToLower()));
        }

        public IQueryable<Story> GetStoriesOrderByField(string field = "storyID", int typeOrderBy = 0)
        {
            return model.Stories.OrderBy(field + " " + (typeOrderBy == 0 ? "ASC" : "DESC"));
        }

        public IQueryable<Story> GetStoriesByCategory(string categorySlug, string status = "")
        {
            IQueryable<Story> stories = model.Stories.Where(s => s.genres.Contains(categorySlug.ToLower()));
            if (status != "")
            {
                stories = stories.Where(s => s.status == status);
            }
            return stories.OrderByDescending(story => story.storyID);
        }

        public IQueryable<Story> GetStoriesByList(string listSlug, string status = "")
        {
            IQueryable<Story> stories = model.Stories;
            switch (listSlug)
            {
                case "truyen-moi-cap-nhat":
                    stories = model.Stories;
                    break;
                case "truyen-hot":
                    stories = model.Stories.Where(s => s.isHot);
                    break;
                case "truyen-full":
                    stories = model.Stories.Where(s => s.status.ToLower() == "full");
                    break;
                case "duoi-100-chuong":
                    stories = model.Stories
                    .Where(s => model.StoryChapters
                        .GroupBy(sc => sc.storySlug)
                        .Where(g => g.Count() < 100)
                        .Select(g => g.Key)
                        .Contains(s.slug));
                    break;
                case "100-500-chuong":
                    stories = model.Stories
                   .Where(s => model.StoryChapters
                       .GroupBy(sc => sc.storySlug)
                       .Where(g => g.Count() >= 100 & g.Count() <= 500)
                       .Select(g => g.Key)
                       .Contains(s.slug));
                    break;
                case "500-1000-chuong":
                    stories = model.Stories
                   .Where(s => model.StoryChapters
                       .GroupBy(sc => sc.storySlug)
                       .Where(g => g.Count() > 500 & g.Count() <= 1000)
                       .Select(g => g.Key)
                       .Contains(s.slug));
                    break;
                case "tren-1000-chuong":
                    stories = model.Stories
                   .Where(s => model.StoryChapters
                       .GroupBy(sc => sc.storySlug)
                       .Where(g => g.Count() > 1000)
                       .Select(g => g.Key)
                       .Contains(s.slug));
                    break;
                default:
                    break;
            }
            if (status != "" && listSlug != "truyen-full")
            {
                stories = stories.Where(s => s.status == status);
            }
            return stories.OrderByDescending(story => story.storyID);
        }

        // story Chapter
        public IQueryable<StoryChapter> GetChapters()
        {
            return model.StoryChapters;
        }
        public IQueryable<StoryChapter> GetChapters(string storySlug)
        {
            string lowercaseSlug = storySlug.ToLower();
            return model.StoryChapters.Where(c => c.storySlug == lowercaseSlug);
        }

        // room
        public IQueryable<Room> GetRooms()
        {
            return model.Rooms;
        }

        public IQueryable<Room> GetRooms(string roomID)
        {
            return model.Rooms.Where(r => r.roomID == Convert.ToInt32(roomID));
        }

        // message
        public IQueryable<Message> GetMessages(int roomID)
        {
            return model.Messages.Where(m => m.roomID == Convert.ToInt32(roomID));
        }

        // user
        public IQueryable<User> GetUsers()
        {
            return model.Users;
        }
        public User GetUserByUserID(int userID)
        {
            return model.Users.Where(u => u.uid == userID).FirstOrDefault();
        }
        public User GetUserByUserName(string username)
        {
            return model.Users.Where(u => u.username == username).FirstOrDefault();
        }

        public int GetRoleUser(int userID)
        {
            return model.Users.Where(u => u.uid == userID).FirstOrDefault().role;
        }
        public int GetRoleUser(string username)
        {
            return model.Users.Where(u => u.username == username).FirstOrDefault().role;
        }

        // userDetail
        public IQueryable<UserDetail> GetUserDetail(string username)
        {
            return model.UserDetail.Where(ud => ud.username == username);
        }


        // insert obj
        public void AddRecord<T>(T obj)
        {
            model.Set(obj.GetType()).Add(obj);
            //model.SaveChanges();
        }

        // update 
        public void SaveChanges()
        {
            model.SaveChanges();
        }
        // delete obj
        public void DeleteRecord<T>(T obj)
        {
            model.Set(obj.GetType()).Remove(obj);
            //model.SaveChanges();
        }
    }
}
