using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using System.Linq;

namespace InstagramClone.Domain.Infrastucture
{
    public class PostRepository : EfRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Post GetByIdWithItems(int Id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == Id);

            if (post != null)
            {
                _dbContext.Entry(post).Reference(p => p.User).Load();
                _dbContext.Entry(post).Collection(p => p.Comments).Load();
                _dbContext.Entry(post).Collection(p => p.Likes).Load();

                foreach (var i in post.Comments)
                {
                    _dbContext.Entry(i).Reference(p => p.User).Load();
                }

                foreach (var i in post.Likes)
                {
                    _dbContext.Entry(i).Reference(p => p.User).Load();
                }
            }

            return post;
        }
    }
}
