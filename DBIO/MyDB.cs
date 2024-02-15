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
        // category
        Model model = new Model();
        public List<Category> GetCategories()
        {
            var _model = new Model();
            return _model.Categories.ToList();
        }

        public IQueryable<Category> GetCategories2()
        {
            Model _model = new Model();
            return _model.Categories;
        }

        // author
        public IQueryable<Author> GetAuthors()
        {
            var _model = new Model();
            return _model.Authors;
        }

        public Author GetAuthor(string authorSlug)
        {
            var _model = new Model();
            return _model.Authors.Where(author => author.slug == authorSlug.ToLower()).FirstOrDefault();
        }

        // story
        public IQueryable<Story> GetStories()
        {
            var _model = new Model();
            return _model.Stories;
        }

        public IQueryable<Story> GetStoriesByStatus(string status)
        {
            Model _model = new Model();
            return _model.Stories.Where(story => story.status.ToLower() == status.ToLower());
        }

        public IQueryable<Story> GetHotStories()
        {
            Model _model = new Model();
            return _model.Stories.Where(story => story.isHot);
        }

        public IQueryable<Story> GetHotStories(string categorySlug)
        {
            var _model = new Model();
            return _model.Stories.Where(story => story.isHot && story.genres.Contains(categorySlug.ToLower()));
        }

        public IQueryable<Story> GetStoriesOrderByField(string field = "storyID", int typeOrderBy = 0)
        {
            var _model = new Model();
            return _model.Stories.OrderBy(field + " " + (typeOrderBy == 0 ? "ASC" : "DESC"));
        }

        public IQueryable<Story> GetStoriesByCategory(string categorySlug, string status = "")
        {
            var _model = new Model();
            IQueryable<Story> stories = _model.Stories.Where(s => s.genres.Contains(categorySlug.ToLower()));
            if (status != "")
            {
                stories = stories.Where(s => s.status == status);
            }
            return stories.OrderByDescending(story => story.storyID);
        }

        public IQueryable<Story> GetStoriesByList(string listSlug, string status = "")
        {
            var _model = new Model();
            IQueryable<Story> stories = _model.Stories;
            switch (listSlug)
            {
                case "truyen-moi-cap-nhat":
                    stories = _model.Stories;
                    break;
                case "truyen-hot":
                    stories = _model.Stories.Where(s => s.isHot);
                    break;
                case "truyen-full":
                    stories = _model.Stories.Where(s => s.status.ToLower() == "full");
                    break;
                case "duoi-100-chuong":
                    stories = _model.Stories
                    .Where(s => _model.StoryChapters
                        .GroupBy(sc => sc.storySlug)
                        .Where(g => g.Count() < 100)
                        .Select(g => g.Key)
                        .Contains(s.slug));
                    break;
                case "100-500-chuong":
                    stories = _model.Stories
                   .Where(s => _model.StoryChapters
                       .GroupBy(sc => sc.storySlug)
                       .Where(g => g.Count() >= 100 & g.Count() <= 500)
                       .Select(g => g.Key)
                       .Contains(s.slug));
                    break;
                case "500-1000-chuong":
                    stories = _model.Stories
                   .Where(s => _model.StoryChapters
                       .GroupBy(sc => sc.storySlug)
                       .Where(g => g.Count() > 500 & g.Count() <= 1000)
                       .Select(g => g.Key)
                       .Contains(s.slug));
                    break;
                case "tren-1000-chuong":
                    stories = _model.Stories
                   .Where(s => _model.StoryChapters
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
            var _model = new Model();
            return _model.StoryChapters;
        }
        public IQueryable<StoryChapter> GetChapters(string storySlug)
        {
            var _model = new Model();
            string lowercaseSlug = storySlug.ToLower();
            return _model.StoryChapters.Where(c => c.storySlug == lowercaseSlug);
        }

        // room
        public IQueryable<Room> GetRooms()
        {
            var _model = new Model();
            return _model.Rooms;
        }

        public IQueryable<Room> GetRooms(string roomID)
        {
            var _model = new Model();
            return _model.Rooms.Where(r => r.roomID == Convert.ToInt32(roomID));
        }

        // message
        public IQueryable<Message> GetMessages()
        {
            var _model = new Model();
            return _model.Messages;
        }
        public IQueryable<Message> GetMessages(int roomID)
        {
            var _model = new Model();
            return _model.Messages.Where(m => m.roomID == roomID);
        }

        // user
        public IQueryable<User> GetUsers()
        {
            var _model = new Model();
            return _model.Users;
        }
        public User GetUserByUserID(int userID)
        {
            return model.Users.Where(u => u.uid == userID).FirstOrDefault();
        }
        public User GetUserByUserName(string username)
        {
            var _model = new Model();
            return model.Users.Where(u => u.username == username).FirstOrDefault();
        }

        public int GetRoleUser(int userID)
        {
            var _model = new Model();
            return _model.Users.Where(u => u.uid == userID).FirstOrDefault().role;
        }
        public int GetRoleUser(string username)
        {
            var _model = new Model();
            return _model.Users.Where(u => u.username == username).FirstOrDefault().role;
        }

        // userDetail
        public UserDetail GetUserDetail(string username)
        {
            return model.UserDetail.Where(ud => ud.username == username).FirstOrDefault();
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
