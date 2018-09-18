using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InstagramClone.Domain.Infrastucture
{
    public class PostRepository : EfRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Post> GetPostsByTag(Tag tag)
        {
            var posts = _dbContext.TagPosts.Include(tp => tp.Tag).Include(tp => tp.Post).ToList();
            var res = posts.Where(tp => tp.Tag.Text == tag.Text).Select(tp => tp.Post).ToList();
            return res;
        }

        public ICollection<Post> GetPostsWithItemsByTag(Tag tag)
        {
            var posts = GetPostsByTag(tag);
            foreach (var i in posts)
            {
                LoadPost(i);
            }
            return posts;
        }

        public Post GetByIdWithItems(int Id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == Id);
            LoadPost(post);

            return post;
        }

        private void LoadPost(Post post)
        {
            if (post != null)
            {
                _dbContext.Entry(post).Reference(p => p.User).Load();
                _dbContext.Entry(post).Collection(p => p.Comments).Load();
                _dbContext.Entry(post).Collection(p => p.Likes).Load();
                _dbContext.Entry(post).Collection(p => p.TagPosts).Load();

                foreach (var i in post.Comments)
                {
                    _dbContext.Entry(i).Reference(p => p.User).Load();
                }

                foreach (var i in post.Likes)
                {
                    _dbContext.Entry(i).Reference(p => p.User).Load();
                }

                foreach (var i in post.TagPosts)
                {
                    _dbContext.Entry(i).Reference(tp => tp.Tag).Load();
                }
            }
        }
    }
}
