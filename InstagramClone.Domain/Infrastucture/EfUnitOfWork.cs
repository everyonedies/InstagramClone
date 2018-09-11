using System.Threading.Tasks;
using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;

namespace InstagramClone.Domain.Infrastucture
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;

        private IUserRepository userRepository;
        private IFollowerRepository followerRepository;
        private IPostRepository postRepository;
        private ILikeRepository likeRepository;

        public EfUnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IUserRepository Users
        {
            get
            {
                return userRepository = userRepository ?? new UserRepository(applicationDbContext);
            }
        }

        public IFollowerRepository Followers
        {
            get
            {
                return followerRepository = followerRepository ?? new FollowerRepository(applicationDbContext);
            }
        }

        public IPostRepository Posts
        {
            get
            {
                return postRepository = postRepository ?? new PostRepository(applicationDbContext);
            }
        }

        public ILikeRepository Likes
        {
            get
            {
                return likeRepository = likeRepository ?? new LikeRepository(applicationDbContext);
            }
        }

        public async Task SaveAsync()
        {
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
