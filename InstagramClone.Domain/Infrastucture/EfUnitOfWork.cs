using System.Threading.Tasks;
using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;

namespace InstagramClone.Domain.Infrastucture
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;

        private IPostRepository postRepository;
        private IUserRepository userRepository;
        private IFollowerRepository followerRepository;

        public EfUnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IPostRepository Posts
        {
            get
            {
                return postRepository = postRepository ?? new PostRepository(applicationDbContext);
            }
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

        public async Task SaveAsync()
        {
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
