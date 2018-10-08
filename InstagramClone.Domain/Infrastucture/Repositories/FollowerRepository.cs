using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Infrastucture
{
    public class FollowerRepository : EfRepository<Follower>, IFollowerRepository
    {
        public FollowerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
