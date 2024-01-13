using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MyDB
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=MyDB")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<CATEGORy> CATEGORIES { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<StoryChapter> StoryChapters { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CATEGORy>()
                .Property(e => e.path)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.coverImage)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.insideImage)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.lastChapter)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.lastChapterSlug)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.genres)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.slug)
                .IsUnicode(false);

            modelBuilder.Entity<StoryChapter>()
                .Property(e => e.storySlug)
                .IsUnicode(false);

            modelBuilder.Entity<StoryChapter>()
                .Property(e => e.slug)
                .IsUnicode(false);

            modelBuilder.Entity<StoryChapter>()
                .Property(e => e.preChapterSlug)
                .IsUnicode(false);

            modelBuilder.Entity<StoryChapter>()
                .Property(e => e.nextChapterSlug)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.hashPwd)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
